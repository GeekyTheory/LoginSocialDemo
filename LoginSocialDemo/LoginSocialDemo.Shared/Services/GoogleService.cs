using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;
using LoginSocialDemo.Models;
using LoginSocialDemo.Services.Interfaces;
using Newtonsoft.Json;

namespace LoginSocialDemo.Services
{
    public class GoogleService : IGoogleService
    {
        private IApplicationDataService applicationSettings;
        public GoogleService(IApplicationDataService applicationSettings)
        {
            this.applicationSettings = applicationSettings;
        }

        public async Task<Session> LoginAsync()
        {
            var googleUrl = new StringBuilder();
            googleUrl.Append("https://accounts.google.com/o/oauth2/auth?client_id=");
            googleUrl.Append(Uri.EscapeDataString(Constants.GoogleClientId));
            googleUrl.Append("&scope=openid%20email%20profile");
            googleUrl.Append("&redirect_uri=");
            googleUrl.Append(Uri.EscapeDataString(Constants.GoogleCallbackUrl));
            googleUrl.Append("&state=foobar");
            googleUrl.Append("&response_type=code");

            var startUri = new Uri(googleUrl.ToString());

#if WINDOWS_APP
            var endUri = new Uri("https://accounts.google.com/o/oauth2/approval?");
            var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.UseTitle, startUri, endUri);
            return await GetSessionAsync(webAuthenticationResult);
#else
            WebAuthenticationBroker.AuthenticateAndContinue(startUri, new Uri(Constants.GoogleCallbackUrl), null, WebAuthenticationOptions.None);
            return null;
#endif
        }


        public async Task<Session> GetSessionAsync(WebAuthenticationResult result)
        {
            if (result.ResponseStatus == WebAuthenticationStatus.Success)
            {
                var code = GetCode(result.ResponseData);
                var serviceRequest = await GetToken(code);

                return new Session
                {
                    AccessToken = serviceRequest.access_token,
                    Id = serviceRequest.id_token,
                    Provider = Constants.GoogleProvider
                };
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

        private string GetCode(string webAuthResultResponseData)
        {
            // Success code=4/izytpEU6PjuO5KKPNWSB4LK3FU1c
            var split = webAuthResultResponseData.Split('&');

            return split.FirstOrDefault(value => value.Contains("code"));
        }

        private static async Task<ServiceResponse> GetToken(string code)
        {

            const string TokenUrl = "https://accounts.google.com/o/oauth2/token";

            var body = new StringBuilder();
            body.Append(code);
            body.Append("&client_id=");
            body.Append(Uri.EscapeDataString(Constants.GoogleClientId));
            body.Append("&client_secret=");
            body.Append(Uri.EscapeDataString(Constants.GoogleClientSecret));
            body.Append("&redirect_uri=");
            body.Append(Uri.EscapeDataString(Constants.GoogleCallbackUrl));
            body.Append("&grant_type=authorization_code");

            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, new Uri(TokenUrl))
            {
                Content = new StringContent(body.ToString(), Encoding.UTF8, "application/x-www-form-urlencoded")
            };
            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var serviceTequest = JsonConvert.DeserializeObject<ServiceResponse>(content);
            return serviceTequest;
        }
    }
}
