using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LoginSocialDemo.Models;
using LoginSocialDemo.Services.Interfaces;
using Microsoft.Live;

namespace LoginSocialDemo.Services
{
    public class MicrosoftService : IMicrosoftService
    {
        private IApplicationDataService applicationSettings;
        private LiveAuthClient authClient;
        private LiveConnectSession liveSession;

        private readonly List<string> scopes;

        public MicrosoftService(IApplicationDataService applicationSettings)
        {
            this.applicationSettings = applicationSettings;

            scopes = new List<string> { "wl.signin", "wl.basic", "wl.offline_access" };
        }

        public async Task<Session> LoginAsync()
        {
            try
            {
                authClient = new LiveAuthClient();
                var loginResult = await authClient.InitializeAsync(scopes);
                var result = await authClient.LoginAsync(scopes);
                if (result.Status == LiveConnectSessionStatus.Connected)
                {
                    liveSession = loginResult.Session;
                    var session = new Session
                    {
                        AccessToken = result.Session.AccessToken,
                        Provider = Constants.MicrosoftProvider,
                    };
                    return session;
                }

            }
            catch (LiveAuthException ex)
            {
                throw new InvalidOperationException("Login canceled.", ex);
            }

            return null;
        }

        public async Task<IDictionary<string, object>> GetUserInfo()
        {
            try
            {
                var liveClient = new LiveConnectClient(liveSession);
                LiveOperationResult operationResult = await liveClient.GetAsync("me");

                return operationResult.Result;
            }
            catch (LiveConnectException e)
            {
            }

            return null;
        }
    }
}
