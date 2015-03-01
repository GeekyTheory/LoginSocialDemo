using System;
using System.Globalization;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;
using LoginSocialDemo.Models;
using LoginSocialDemo.Services.Interfaces;

namespace LoginSocialDemo.Services
{
    public class SessionService : ISessionService
    {
        private readonly IApplicationDataService applicationSettings;
        private readonly IFacebookService facebookService;
        private readonly IMicrosoftService microsoftService;
        private readonly IGoogleService googleService;


        public SessionService(IApplicationDataService applicationSettings, IFacebookService facebookService,
            IMicrosoftService microsoftService, IGoogleService googleService)
        {
            this.applicationSettings = applicationSettings;
            this.facebookService = facebookService;
            this.microsoftService = microsoftService;
            this.googleService = googleService;
        }

        public string Provider { get; set; }

        /// <summary>
        /// Windows Phone 8.1 Only: MainViewModel Continue Method.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public async Task<Session> GetSession(WebAuthenticationResult result)
        {
            try
            {
                Session session = null;
                switch (Provider)
                {
                    case Constants.FacebookProvider:
                        session = facebookService.GetSession(result);
                        break;
                    case Constants.GoogleProvider:
                        session = await googleService.GetSessionAsync(result);
                        break;
                }
                if (session == null)
                {
                    return null;
                }
                Save(session);
                return session;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        public async Task<Session> LoginAsync(string provider)
        {
            Provider = provider;
            try
            {
                Session session = null;
                switch (provider)
                {
                    case Constants.FacebookProvider:
                        session = await facebookService.LoginAsync();
                        break;
                    case Constants.MicrosoftProvider:
                        session = await microsoftService.LoginAsync();
                        break;
                    case Constants.GoogleProvider:
                        session = await googleService.LoginAsync();
                        break;
                }
                if (session == null)
                {
                    return null;
                }
                Save(session);
                return session;
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        public void Save(Session session)
        {
            applicationSettings.SaveCredential(session.Provider, session.ExpireDate, session.AccessToken);
        }
    }
}
