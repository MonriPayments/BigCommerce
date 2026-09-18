using BigCommerceApi.Domain.Model.Stores;
using BigCommerceApi.Domain.Model.Users;
using BigCommerceApi.Domain.Services.Callback.SaveTransaction;
using System;

namespace BigCommerceApi.Domain.Services.Helpers
{
    public static class UserHelper
    {
        public static void PopulateUserTokens(SaveCallbackCommand command, User user, ref UserTokens userToken)
        {
            userToken.TokenCreated = DateTime.Now;
            userToken.Token = Guid.Parse(command.Token!);
            userToken.TokenNumber = command.TokenNumber;
            userToken.PaymentType = command.CreditCardName;
            userToken.MaskedPan = command.CreditCardNumber;
            userToken.CreditCardExpirationDate = GetExpirationDate(command.ExpirationDate!);
            userToken.User = user;
        }

        public static void PopulateUser(StoreConfiguration storeConfiguration, string email, ref User user)
        {
            user.StoreConfiguration = storeConfiguration;
            user.Email = email;
        }

        public static DateTime GetExpirationDate(string yyMM)
        {
            // Parse the input string to get year and month
            int year = int.Parse(yyMM.Substring(0, 2));
            int month = int.Parse(yyMM.Substring(2, 2));

            // Handle year, assuming year 2000-2099 range
            year += (year >= 0 && year <= 99) ? 2000 : 1900;

            // Get the first day of the month
            DateTime firstDayOfMonth = new DateTime(year, month, 1);

            // Calculate the last day of the month
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            // Set the time to the last second of the last day
            DateTime lastMomentOfMonth = new DateTime(
                lastDayOfMonth.Year,
                lastDayOfMonth.Month,
                lastDayOfMonth.Day,
                23, 59, 59,
                999  // If you need millisecond precision
            );

            return lastMomentOfMonth;
        }
    }
}
