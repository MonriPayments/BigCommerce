# BigCommerce + WSPay Integration

An ASP.NET Core service that lets online shops running on **BigCommerce** accept
credit-card payments through **WSPay** (a payment processor common in the region).
It sits between the BigCommerce checkout and WSPay: the shop owner configures it
once, and from then on it keeps the two sides in sync for every payment, refund,
and saved card.

It is **not** a public API and **not** consumer-facing. Its only HTTP clients are:

- the BigCommerce checkout page (via JavaScript this app serves and injects),
- WSPay (which calls back with payment results), and
- an internal admin UI (which lives in a separate repository).

> **New to this project?** Read this file first, then the two guides in
> [`docs/`](/Source/docs/) — a plain-language [overview](Source/docs/bigcommerceapi-overview.html)
> and a detailed [developer guide](Source/docs/bigcommerceapi-developer.html) (also
> available as PDFs). The developer guide is the authoritative deep-dive; this
> README is the quick-start and orientation.

---

## What it does

1. A customer reaches the BigCommerce checkout and picks the "Pay by card" option
   (the label is configurable per store, default `Plaćanje karticama`).
2. Depending on how the store is configured, the customer either:
   - **Embedded mode** — enters card details in an iframe inside the checkout page, or
   - **Redirect mode** — is sent to a WSPay-hosted page, then bounced back.
3. Customers can optionally **save a card** — the app stores only a WSPay-issued
   token, never the card number.
4. WSPay processes the payment and calls back. The app updates the BigCommerce
   order status (Completed / Cancelled) and handles refunds and voids.

**What it deliberately does not do:** it does not take the money (WSPay does), does
not store card numbers, and does not run the shop (catalogue, shipping, taxes, and
emails all stay in BigCommerce).

---

## Tech stack

| Concern            | Choice |
|--------------------|--------|
| Runtime            | ASP.NET Core on **.NET 10** |
| Web host           | `Client/Web` (`BigCommerceApi.Client.Web`) |
| DI                 | **Autofac** (modules: `Domain.Services`, `Client.Web`, `Persistency.NHibernate`) |
| Persistence        | **NHibernate** via FluentNHibernate, **SQL Server** backend |
| Schema migrations  | **FluentMigrator** (`Persistency/Migrations`) |
| App framework      | **WebStudio** (internal): `IQueryExecutor`, `ICommandExecutor`, `UnitOfWork`, `Response<T>`, command/query handler pattern — *not* MediatR |
| Outbound HTTP      | RestSharp (BigCommerce REST Management API, WSPay form API) |
| Admin auth         | JWT bearer |
| Front-end glue     | Vanilla JS injected into the BigCommerce checkout DOM (`Client/Web/wwwroot`) |

---

## Solution layout

```
BigCommerceApi.sln
├── Domain/
│   ├── Model/            Entities: StoreConfiguration, Shops, Transaction, User/UserTokens, Administrators, Logs
│   ├── Services/         Command/query handlers, business logic, external API clients
│   │   ├── Callback/         SaveCallback, OrderStatusUpdate, SaveTransaction
│   │   ├── Checkout/         InitiateCheckout, GetIFrameValues
│   │   ├── WSPayForm/        Outbound WSPay form-API client (IFormWSPay)
│   │   ├── WSPay/            WSPay backoffice ops (void/refund), signature generation
│   │   ├── RestManagementApi/ BigCommerce REST client
│   │   └── Helpers/          WsPayFormHelper, TransactionsHelpers, caching, error messages
│   └── Projections/      Read-side query DTOs
├── Persistency/
│   ├── NHibernate/       Mappings + query handlers (the active persistence layer)
│   ├── Migrations/       FluentMigrator migrations + SQL scripts
│   └── EntityFramework/  Present but not wired up in Program.cs (NHibernate is used at runtime)
├── Client/
│   ├── Web/              Controllers, request/response DTOs, Program.cs, appsettings.*, wwwroot/
│   └── Console/          Scratch console harness (not part of the .sln)
└── docs/                 Overview + developer guide (HTML + PDF)
```

### Key entities

- **StoreConfiguration** — one per BigCommerce store. Holds the BigCommerce
  `AccessToken`, API path, merchant site URL, `StoreStatus` (Test/Production),
  `CheckoutType` (EmbeddedCheckout/Redirect), payment-method label, order-confirmation
  redirect route, and default order status.
- **Shops** — N rows per StoreConfiguration, each with WSPay credentials (`ShopID`,
  `SecretKey`, `Language`, `IsTokenShopId`). ⚠️ `IsTokenShopId` is a WSPay quirk: it
  means "this row is the tokenization-enabled WSPay shop," **not** "this row stores a token."
- **Transaction** — created/updated on every WSPay callback, keyed by
  `(ShoppingCartID, ShopID)`.
- **User + UserTokens** — saved-card records, keyed by `(customer email, StoreConfiguration)`.

---

## Getting started

### Prerequisites

- **.NET SDK 10.0.102+**
- **SQL Server** reachable from your machine (a dev instance is fine)
- Access to the internal **WebStudio** NuGet feed (the `WebStudio.*` packages are
  private — make sure your NuGet sources include it, or restore will fail)
- Visual Studio 2022 (17.5+) or the `dotnet` CLI

### Build & run

```bash
# from the Source/ directory
dotnet restore BigCommerceApi.sln
dotnet build BigCommerceApi.sln
dotnet run --project Client/Web/Client.Web.csproj
```

The web project defines these build configurations: `Debug`, `Release`, `Test`, `Dev`.

In Development, Swagger UI is available at `/swagger`. A health check lives at the
`PingController` endpoint.

### Configuration

Infrastructure config lives in `Client/Web/appsettings.json` (with
`appsettings.Development.json` and `appsettings.Test.json` overrides). Key sections:

| Section | Purpose |
|---------|---------|
| `DatabaseConfiguration` | SQL Server connection string + default schema |
| `MSSQLServerLoggerConfiguration` | Log-table sink (structured logs go to a SQL `Log` table) |
| `FormWSPayConfiguration` | WSPay form-API base URLs, iframe authorization URLs, timeouts |
| `WSPayPaymentGatewayConfiguration` | WSPay backoffice URLs + basic auth |
| `BigCommerceConfiguration` | OAuth client id/secret + `AppUri` (this service's public hostname) |
| `JwtSettings` | Admin authentication |

**Per-store data lives in the database** (StoreConfiguration + Shops), not in config.

> ### ⚠️ Rotate the committed credentials before handover
> `appsettings.json` currently contains **real-looking secrets** checked into the
> repository — a SQL Server password, the BigCommerce app client id/secret, and
> WSPay basic-auth credentials. As part of taking this project over you should:
> 1. **Rotate every credential** in `appsettings.json` (DB password, BigCommerce
>    `AppClientSecret`, WSPay auth), since they have been exposed in source control.
> 2. Move secrets out of the committed file — use user-secrets, environment
>    variables, or a secrets manager — and keep only non-secret defaults in git.

### Database migrations

Schema is managed by **FluentMigrator** in `Persistency/Migrations` (migration
classes named `M<timestamp>_*.cs`, plus embedded SQL scripts under
`Scripts/` for stored procedures and functions). Point the migration runner at your
target database's connection string and apply migrations before first run. The
`dotnet-ef` tool is also pinned in `Client/Web/dotnet-tools.json`, but NHibernate is
the persistence layer actually wired up at runtime.

---

## Runtime flows (quick reference)

- **Checkout detection** — `POST /checkout/check-merchant-integration` returns
  `EmbeddedCheckout` or `Redirect`; `POST /checkout/fetch-payment-method-name`
  returns the radio-button label.
- **Embedded** — `POST /checkout/get-iframe-values` builds one WSPay iframe payload
  per Shops row; the JS submits it into the `wspay-iframe`.
- **Redirect** — `POST /checkout/init` creates the WSPay form transaction and
  returns a `PaymentFormUrl`; per-order context is cached (~30 min TTL).
- **Callbacks** — WSPay calls `POST /callback/savetransaction` (upserts the
  Transaction, upserts saved-card records, updates the BigCommerce order) and
  `POST /callback/orderstatusupdate` (void/refund from the backoffice).
- **Post-payment redirect** — WSPay sends the customer to
  `{AppUri}/redirect/order-confirmation`; `RedirectController` looks up cached order
  context and bounces back to the store's configured confirmation route.
- **Admin** — `AdministratorController` at `/administrator` (JWT-protected after
  `/login`): CRUD on StoreConfiguration/Shops, read-side queries, and log search.

Controllers live in `Client/Web/Controllers/`: `Auth`, `Callback`, `Checkout`,
`Customer`, `Redirect`, `Setup`, `TransactionStatus`, `Administrator`, `Ping`, `Error`.

---

## Things to know before you change anything

The codebase is **WSPay-coupled throughout**. If you ever need to support a second
gateway, these are the first places you'll touch (details in the developer guide):

- `WsPayFormHelper.CreateIframeRequest` builds the WSPay-shaped iframe payload and
  hardcodes the form URLs. It also contains a **hardcoded per-merchant override**
  (`shop?.ShopID == "SKOLSKA" ? "self" : "TOP"` for the iframe response target).
- `SaveCallbackHandler` hardcodes the payment-method label `"WSPay by Monri"` when
  updating BigCommerce orders.
- `InitiateCheckoutHandler` picks the WSPay form URL from config based on `StoreStatus`.
- The callback DTOs (`ProcessingTransactionCallbackRequest`, `SaveCallbackCommand`)
  are WSPay-shaped (`WsPayOrderId`, `STAN`, `ApprovalCode`, `Signature`, `ECI`, …).
- `wwwroot` JavaScript is WSPay-coupled (element IDs like `wspay-iframe`,
  `wspayiframeshopID`, `wspayiframesignature`) and mixes the embedded and redirect
  branches based on the integration-detection response.
- `Shops.IsTokenShopId` has WSPay-specific semantics (see the entity note above).

## Deployment

Single-host ASP.NET Core app. Static `wwwroot` serves the JavaScript shipped to
BigCommerce. The app's public hostname (`BigCommerceConfiguration:AppUri`) is both
what gets injected into BigCommerce pages **and** what WSPay calls back to — so it
must be publicly reachable and correct per environment. The default `appsettings.json`
points at `monribigcomm.wspay.info`. Publish profiles for `Test` and `Production`
live in `Client/Web/Properties/PublishProfiles/`.

## Not covered here

- The admin UI's UX (lives in a different repository).
- WSPay onboarding (merchant agreement, API credentialing).
- The BigCommerce app install / OAuth handshake (handled by `AuthController`, but a
  separate concern from runtime checkout).

See [`docs/bigcommerceapi-developer.html`](Source/docs/bigcommerceapi-developer.html) for
the full architecture and flow diagrams.
