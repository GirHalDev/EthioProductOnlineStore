using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace EthioProductShoppingCenter.BusinessLogic
{
    public class PayPalConfiguration
    {
        public readonly static string clientId;
        public readonly static string clientSecret;

        static PayPalConfiguration()
        {
            var config = GetConfig();
            clientId = "AbRRsZUCfoK5voST_0NYF50zbvqa7QY3NZ4ZcLPmWaoj0IGKIi5E5fsq8NFqq3iMBX-ZAaeg5srZb_qu";
            clientSecret = "EP5uEdmDUuwVY9JOS7pETQ57mJVkPMWif6KDl-tKNrAmovie9zI61zXyqvPk2np-iKqUlsQv3Na-ITxg";
        }

        private static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(clientId, clientSecret, GetConfig()).GetAccessToken();
            return accessToken;

        }

        public static APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
    }
}