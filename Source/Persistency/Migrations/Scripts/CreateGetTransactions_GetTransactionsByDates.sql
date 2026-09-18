SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
--DROP PROCEDURE [dbo].[Transactions_GetTransactionsByDates]
CREATE PROCEDURE [dbo].[Transactions_GetTransactionsByDates]
(
	@DateFrom DateTime,
	@DateTo DateTime,
	@PageSize integer,
	@PageNumber integer
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TotalRows integer;
	DECLARE @TotalPages integer;

	-- Calculate the total number of rows
	SELECT 
		@TotalRows = COUNT(*)
	FROM 
		[BigCommerceApi.local].[dbo].[Transaction]
	WHERE 
		TransactionDateTime BETWEEN @DateFrom AND @DateTo;

	-- Calculate the total number of pages
	SET @TotalPages = CEILING(CAST(@TotalRows AS float) / @PageSize);

	-- Retrieve the paginated result set
	SELECT 
		[Id],
		[WsPayOrderId],
		[UniqueTransactionNumber],
		[Signature],
		[STAN],
		[ApprovalCode],
		[ErrorMessage],
		[ShopID],
		[ShoppingCartID],
		[Amount],
		[CurrencyCode],
		[Success],
		[Authorized],
		[Completed],
		[Voided],
		[Refunded],
		[PaymentPlan],
		[Partner],
		[OnSite],
		[CreditCardName],
		[CreditCardNumber],
		[ECI],
		[CustomerFirstName],
		[CustomerLastName],
		[CustomerAddress],
		[CustomerCity],
		[CustomerCountry],
		[CustomerPhone],
		[CustomerZIP],
		[CustomerEmail],
		[TransactionDateTime],
		[Token],
		[TokenNumber],
		[ExpirationDate],
		@TotalPages as TotalPages
	FROM 
		[BigCommerceApi.local].[dbo].[Transaction]
	WHERE 
		TransactionDateTime BETWEEN @DateFrom AND @DateTo
	ORDER BY 
		TransactionDateTime DESC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;
END
GO
