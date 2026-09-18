using System;
using System.Collections.Generic;
using BigCommerceApi.Domain.Model.Users;

namespace BigCommerceApi.Domain.Services.Helpers
{
    public static class CustomerTokensHelper
    {
        const string imagesPath = "assets/images/credit-card-logos/";
        const string visaLogo = "Visa50.gif";
        const string amexLogo = "AmericanExpress50.jpg";
        const string dinersLogo = "Diners50.gif";
        const string mastercardLogo = "MasterCard50.gif";
        const string maestroLogo = "maestro50.gif";

        public static List<CustomerTokens.CustomerTokens> GetCustomerTokensList(List<UserTokens> userTokens, string AppUri)
        {
            var customerTokenList = new List<CustomerTokens.CustomerTokens>();

            foreach (var userToken in userTokens)
            {
                CustomerTokens.CustomerTokens customerTokens = new CustomerTokens.CustomerTokens();
                customerTokens.Token = userToken.Token.ToString();
                customerTokens.TokenNumber = userToken.TokenNumber;
                customerTokens.MaskedPan = userToken.MaskedPan;
                customerTokens.PaymentType = userToken.PaymentType;
                customerTokens.ImageSource = AppUri + imagesPath + GetImagePath(userToken.PaymentType!);

                customerTokenList.Add(customerTokens);
            }

            return customerTokenList;
        }

        public static string GetImagePath(string paymentType)
        {
            switch(paymentType)
            {
                case "VISA":
                    return visaLogo;
                case "MAESTRO":
                    return maestroLogo;
                case "MASTERCARD":
                    return mastercardLogo;
                case "DINERS":
                    return dinersLogo;
                case "AMEX":
                    return amexLogo;
                default:
                    return String.Empty;
            }
        }
    }
}
