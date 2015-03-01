using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Data.Json;
using Windows.Security.Authentication.Web;
using LoginSocialDemo.Models;
using LoginSocialDemo.Services.Interfaces;

namespace LoginSocialDemo.Services
{
    public class FacebookService : IFacebookService
    {
        private IApplicationDataService applicationSettings;
        public FacebookService(IApplicationDataService applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }

        public async Task<Session> LoginAsync()
        {
            const string FacebookCallbackUrl = "https://m.facebook.com/connect/login_success.html";
            var facebookUrl = "https://www.facebook.com/dialog/oauth?client_id=" + Uri.EscapeDataString(Constants.FacebookAppId) + "&redirect_uri=" + Uri.EscapeDataString(FacebookCallbackUrl) + "&scope=public_profile,email&display=popup&response_type=token";

            var startUri = new Uri(facebookUrl);
            var endUri = new Uri(FacebookCallbackUrl);

#if WINDOWS_PHONE_APP
            WebAuthenticationBroker.AuthenticateAndContinue(startUri, endUri, null, WebAuthenticationOptions.None);
            return null;
#else
            var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri, endUri);
            return GetSession(webAuthenticationResult);
#endif
        }

        public Session GetSession(WebAuthenticationResult result)
        {
            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                if (result.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    string responseCode = result.ResponseData;
                    responseCode = responseCode.Substring(responseCode.IndexOf("=") + 1);

                    return new Session
                    {
                        AccessToken = responseCode.Split('&').First(),
                        ExpireDate = int.Parse(responseCode.Split('&').Last().Split('=').Last()),
                        Provider = Constants.FacebookProvider
                    };
                }
            }
            if (result.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
            {
                throw new Exception("Error http");
            }
            if (result.ResponseStatus == WebAuthenticationStatus.UserCancel)
            {
                throw new Exception("User Canceled.");
            }
            return null;
        }

        private async Task<UserInfo> GetFacebookUserNameAsync(string accessToken)
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(new Uri("https://graph.facebook.com/me?access_token=" + accessToken));
            var value = JsonValue.Parse(response).GetObject();
            var facebookUserName = value.GetNamedString("name");

            return new UserInfo
            {
                Name = facebookUserName,
            };
        }
    }
}
