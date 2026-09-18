using System;

namespace BigCommerceApi.Domain.Services.Helpers
{
    public static class CurrencyCodeHelper
    {

        public static string[,] CurrenyCodesList = new string[20, 4]
        {
            { "EUR", "978", "€", "2" }, { "USD", "840", "$", "2" }, { "CAD", "124", "$", "2" },
            { "BAM", "977", "BAM", "2" }, { "RSD", "941", "RSD", "2" }, { "GBP", "826", "£", "2" },
            { "AUD", "36", "AUD", "2" }, { "HUF", "348", "Ft", "2" }, { "CZK", "203", "Kč", "2" },
            { "ZAR", "710", "ZAR", "2" }, { "BGN", "975", "лв", "2" }, { "RON", "946", "lei", "2" },
            { "CHF", "756", "CHF", "2" }, { "SEK", "752", "kr", "2" }, { "PLN", "985", "zł", "2" },
            { "HRK", "191", "Kn", "2" }, { "JPY", "392", "¥", "2"}, { "NOK", "578", "kr", "2" },
            { "DKK", "208", "kr", "2" }, { "MKD", "807", "ден", "2" }
        };

        public static string getAlphabeticCode(string? NumericCode)
        {
            string result;
            string numericCode = Convert.ToString(NumericCode!);
            if (!getElementFromCurrenyCodesList(numericCode, 1, 0, out result))
            {
                return ErrorMessages.AlphabeticCodeNotFound;
            }

            return result;
        }

        public static string getNumericCode(string alphabeticCode)
        {
            string result;

            if (!getElementFromCurrenyCodesList(alphabeticCode, 0, 1, out result))
            {
                return ErrorMessages.NumericCodeNotFound;
            }

            return result;
        }

        private static bool getElementFromCurrenyCodesList(string seachValue, int searchByEl, int searchForEl, out string result)
        {
            result = null;

            try
            {
                for (int count = 0; count < CurrenyCodesList.GetLength(0); count++)
                {
                    //contains the vlues of one currency
                    if (CurrenyCodesList[count, searchByEl] == seachValue)
                    {
                        result = CurrenyCodesList[count, searchForEl];
                        return true;
                    }
                }
            }
            catch
            {
            }
            return false;
        }

        public static string getCurrencySign(string numericCode)
        {
            string result;
            if (!getElementFromCurrenyCodesList(numericCode, 1, 2, out result))
            {
                return null;
            }

            return result;

        }
    }
}
