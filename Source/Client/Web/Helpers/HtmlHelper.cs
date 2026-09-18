namespace BigCommerceApi.Client.Web.Helpers
{
    public static class HtmlHelper
    {
        public static string GetNavbarHtml(string activePage)
        {
            return $@"
            <nav class='navbar bg-light px-4 py-2'>
                <a href='#' class='navbar-brand'><img src=""/assets/images/monri-logo.svg"" alt=""Monri Payments logo""></a>
                <ul class='nav'>
                    <li class='nav-item'><a href='/auth/order-details' class='nav-link {(activePage == "OrderDetails" ? "active" : "")}'>Transaction List</a></li>
                </ul>
            </nav>";
        }

        public static string GetHtmlHeader(string title)
        {
            return $@"
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <link href='/assets/css/bootstrap.min.css' rel='stylesheet'>
                <link href=""https://api.fontshare.com/v2/css?f[]=satoshi@700,500,400&display=swap"" rel=""stylesheet"">
                <link rel=""stylesheet"" href=""https://cdn.jsdelivr.net/npm/bootstrap-datepicker@1.9.0/dist/css/bootstrap-datepicker.min.css"">
                <script src=""https://code.jquery.com/jquery-3.6.0.min.js""></script>
                <script src=""https://cdn.jsdelivr.net/npm/bootstrap-datepicker@1.9.0/dist/js/bootstrap-datepicker.min.js""></script>
                <title>{title}</title>
            </head>
        ";
        }

        public static string GetScriptSection(string appUri)
        {
            return $@"
            <script>
                $(document).ready(function() {{
                    $('#endDate').datepicker({{
                        format: 'dd.mm.yyyy',
                        autoclose: true,
                        todayHighlight: true,
                        orientation: 'bottom auto'
                    }});
                }});
                $(document).ready(function() {{
                    $('#startDate').datepicker({{
                        format: 'dd.mm.yyyy',
                        autoclose: true,
                        todayHighlight: true,
                        orientation: 'bottom auto'
                    }});
                }});
            </script>
            <script>
                function getTransactions() {{
                    const startDateValue = document.getElementById('startDate').value;                         
                    const endDateValue = document.getElementById('endDate').value;
                    const customerNameValue = document.getElementById('customerName').value;
                    const shoppingCartIDValue = document.getElementById('shoppingCartID').value;
                    const creditCardNameValue = document.getElementById('creditCardName').value;
                    const shopIDElement = document.getElementById('shopID');
                    let shopIDValue = shopIDElement.value;

                    if (!shopIDValue) {{
                        shopIDValue = Array.from(shopIDElement.options)
                            .filter(option => option.value)
                            .map(option => option.value)
                            .join('|');
                    }}

                    const startDate = formatDate(startDateValue);
                    const endDate = formatDate(endDateValue, true);
                    const pageNumber = 1;
                    const pageSize = 10;

                    const queryParams = new URLSearchParams({{
                        startDate: startDate,
                        endDate: endDate,
                        customerName: customerNameValue,
                        shoppingCartID: shoppingCartIDValue,
                        creditCardName: creditCardNameValue,
                        shopID: shopIDValue,
                        pageNumber: pageNumber,
                        pageSize: pageSize,
                    }}).toString();

                    console.log(queryParams);

                    const url = '{appUri}auth/get-transactions?' + queryParams;
                    console.log(url);
                    fetch(url, {{
                        method: 'GET',
                        headers: {{
                            'Content-Type': 'application/json'
                        }}
                    }})
                    .then(response => {{
                        if (!response.ok) {{
                            throw new Error('Network response was not ok');
                        }}
                        return response.json(); // Parse the JSON response
                    }})
                    .then(data => {{
                        console.log('Order Details:', data);
                        updateTable(data); 
                    }})
                    .catch(error => {{
                        console.error('There was a problem with the fetch operation:', error);
                    }});
                }}

                function formatDate(dateString, isEndDate = false) {{
                    const parts = dateString.split('.');
                    const day = parts[0];
                    const month = parts[1];
                    const year = parts[2];
                    let formattedDate = `${{year}}-${{month}}-${{day}}`;

                    if (isEndDate) {{
                        formattedDate += 'T23:59:59';
                    }} else {{
                        formattedDate += 'T00:00:00';
                    }}

                    return formattedDate;
                }}

                function updateTable(transactions) {{
                    const tableBody = document.querySelector('table.table tbody');
                    tableBody.innerHTML = ''; // Clear the current rows

                    transactions.forEach(transaction => {{
                        const row = document.createElement('tr');
                        const date = new Date(transaction.transactionDateTime);
                        const formattedDate = `${{String(date.getDate()).padStart(2, '0')}}.${{String(date.getMonth() + 1).padStart(2, '0')}}.${{date.getFullYear()}} ${{String(date.getHours()).padStart(2, '0')}}:${{String(date.getMinutes()).padStart(2, '0')}}`;

                        row.innerHTML = `
                            <td>${{formattedDate}}</td>
                            <td>${{transaction.shopID}}</td>
                            <td>${{transaction.customerName}}</td>
                            <td><a href=""${{document.referrer}}manage/orders/${{transaction.shoppingCartID}}"">${{transaction.shoppingCartID}}</a></td>
                            <td>${{transaction.creditCardName}}</td>
                            <td class=""text-end"">${{transaction.amount.toFixed(2)}}</td>
                            <td class=""text-center"">${{transaction.authorized ? ""&#x2714;"" : ""&#x2212;""}}</td>
                            <td class=""text-center"">${{transaction.completed ? ""&#x2714;"" : ""&#x2212;""}}</td>
                            <td class=""text-center"">${{transaction.voided ? ""&#x2714;"" : ""&#x2212;""}}</td>
                            <td class=""text-center"">${{transaction.refunded ? ""&#x2714;"" : ""&#x2212;""}}</td>
                        `;

                        tableBody.appendChild(row); // Append the new row to the table body
                    }});
                }}
            </script>
        ";
        }

        public static string GetTableHead()
        {
            return $@"
            <thead>
                <tr>
                    <th class=""text-start"">Date</th>
                    <th class=""text-start"">ShopID</th>
                    <th class=""text-start"">Customer Name</th>
                    <th class=""text-start"">ShoppingCartID</th>
                    <th class=""text-start"">Credit card</th>
                    <th class=""text-center"">Amount</th>
                    <th class=""text-center"">Authorized</th>
                    <th class=""text-center"">Completed</th>
                    <th class=""text-center"">Voided</th>
                    <th class=""text-center"">Refunded</th>
                </tr>
            </thead>
        ";
        }

        public static string GetSitePropertiesFields(
            string appUri, 
            string merchantPath,           
            string? merchantName = "", 
            string? merchantEmail = "", 
            string? merchantSite = "",
            bool isProduction = false,
            string checkoutType = "EmbeddedCheckout",
            string paymentMethodName = "")
        {
            return $@"
                <form method='post' action='/auth/savemerchantproperties'>
                    <div class='form-group row'>
                        <div class='col-md-2'>
                            <label for='merchantName'>Merchant Name:</label>
                            <input type='text' id='merchantName' name='merchantName' class='form-control' placeholder='Enter your merchant name' value='{merchantName}'>
                        </div>
                        <div class='col-md-2'>
                            <label for='merchantEmail'>Merchant Email:</label>
                            <input type='email' id='merchantEmail' name='merchantEmail' class='form-control' placeholder='Enter your email' value='{merchantEmail}'>
                        </div>
                        <div class='col-md-2'>
                            <label for='paymentMethodName'>Payment method name:</label>
                            <input type='text' id='paymentMethodName' name='paymentMethodName' class='form-control' required placeholder='Payment method name' value='{paymentMethodName}'>
                        </div>
                        <div class='col-md-2'>
                            <label for='merchantSite'>Merchant Site Url:</label>
                            <input type='text' id='merchantSite' name='merchantSite' class='form-control' required placeholder='Enter your site url' value='{merchantSite}'>
                        </div>
                        <div class='col-md-2'>
                            <label for='isProductionToggle' class='form-check-label'>Environment:</label>
                            <div class='form-check form-switch'>
                                <input class='form-check-input' type='checkbox' id='isProductionToggle' name='isProduction' value='true' {(isProduction ? "checked" : "")}>
                                <label class='form-check-label' for='isProductionToggle'>Production Mode</label>
                            </div>
                        </div>
                        <div class='col-md-2'>
                            <label>Checkout Type:</label>
                            <div class='form-check'>
                                <input class='form-check-input' type='radio' name='checkoutType' id='embedded' value='EmbeddedCheckout' {(checkoutType == "EmbeddedCheckout" ? "checked" : "")}>
                                <label class='form-check-label' for='embedded'>Embedded Checkout</label>
                            </div>
                            <div class='form-check'>
                                <input class='form-check-input' type='radio' name='checkoutType' id='redirect' value='Redirect' {(checkoutType == "Redirect" ? "checked" : "")}>
                                <label class='form-check-label' for='redirect'>Redirect</label>
                            </div>
                        </div>
                        <input type='hidden' id='merchantPath' name='merchantPath' value='{merchantPath}'>
                        <div class='row'>
                            <div class='col-md-2 mt-4'>
                                <button type='submit' class='btn btn-secondary'>Save</button>
                            </div>
                        </div>
                    </div>
                </form>
                <span>
                    If you do not see any ShopIDs on your ShopID list or you do not see our payment method on checkout, it means you haven't contacted us yet to complete your connection with WSPay by Monri.<br> 
                    Please reach out to us at wspay@wspay.info and include your BigCommerce store URL in the email.
                    <a href=""{appUri}/WSPayByMonri-BigCommerceUserGuide.pdf"" target=""_blank"">Here is a link to our user guide.</a>
                    Thank you!
                </span>
            ";
        }


        public static string GetForm(
            DateTime startDate, 
            DateTime endDate,
            string shopIDOptions,
            string customerName,
            string shoppingCartID,
            string creditCardName)
        {
            return $@"
            <form method='get' action='/auth/order-details'>
                <div class ='form-group row'>
                    <div class='col-md-2'>
                        <label for='startDate'>Start Date:</label>
                        <input type='text' id='startDate' name='startDate' class='form-control' required placeholder='dd.mm.yyyy'>
                    </div>
                    <div class='col-md-2'>
                        <label for='endDate'>End Date:</label>
                        <input type='text' id='endDate' name='endDate' class='form-control' required placeholder='dd.mm.yyyy'>
                    </div>
                    <div class='col-md-2'>
                        <label for='shopID'>Shop ID:</label>
                        <select id='shopID' name='shopID' class='form-control' id='shopID'>
                            <option value=''>All Shops</option>
                            {shopIDOptions}
                        </select>
                    </div>
                    <div class='col-md-2'>
                        <label for='customerName'>Customer Name:</label>
                        <input type='text' id='customerName' name='customerName' class='form-control' value='{customerName}'>
                    </div>
                    <div class='col-md-2'>
                        <label for='shoppingCartID'>Shopping Cart ID:</label>
                        <input type='text' id='shoppingCartID' name='shoppingCartID' class='form-control' value='{shoppingCartID}'>
                    </div>
                    <div class='col-md-2'>
                        <label for='creditCardName'>Credit Card Name:</label>
                        <input type='text' id='creditCardName' name='creditCardName' class='form-control' value='{creditCardName}'>
                    </div>
                    <div class='col-md-2 mt-4'>
                        <button type='button' class='btn btn-secondary' onclick='getTransactions()'>Search</button>
                    </div>
                </div>                         
            </form>
        ";
        }
    }
}
