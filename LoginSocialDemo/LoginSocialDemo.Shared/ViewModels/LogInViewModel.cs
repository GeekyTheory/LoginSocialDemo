using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;
using Windows.UI.Xaml.Navigation;
using LoginSocialDemo.Models;
using LoginSocialDemo.Services;
using LoginSocialDemo.Services.Interfaces;
using LoginSocialDemo.ViewModels.Base;
using LoginSocialDemo.Views;
using Cimbalino.Toolkit.Services;
using Microsoft.Live;

namespace LoginSocialDemo.ViewModels
{
    public class LogInViewModel : ViewModelBase, IContinuationAwareViewModel
    {
        private readonly INavigationService navigationService;
        private readonly ISessionService sessionService;
        private readonly IMessageBoxService messageBoxService;
        private readonly INetworkInformationService networkInformationService;
        private IApplicationDataService applicationSettings;

        private bool inProgress;
        private bool isFacebookConnected;
        private bool isGoogleConnected;
        private bool isMicrosoftConnected;

        public LogInViewModel(INavigationService navigationService, ISessionService sessionService, 
            IMessageBoxService messageBoxService, INetworkInformationService networkInformationService,
            IApplicationDataService applicationSettings)
        {
            this.navigationService = navigationService;
            this.sessionService = sessionService;
            this.messageBoxService = messageBoxService;
            this.networkInformationService = networkInformationService;
            this.applicationSettings = applicationSettings;

            LogInCommand = new DelegateCommand<string>(LogInCommandExec);
#if WINDOWS_APP
            GoBackCommand = new DelegateCommand(GoBackCommandExec);
#endif
        }

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is List<Session>)
            {
                var sessions = (List<Session>) e.Parameter;
                if (sessions != null)
                {
                    foreach (var session in sessions)
                    {
                        switch (session.Provider)
                        {
                            case Constants.FacebookProvider:
                                IsFacebookConnected = true;
                                break;
                            case Constants.GoogleProvider:
                                IsGoogleConnected = true;
                                break;
                            case Constants.MicrosoftProvider:
                                IsMicrosoftConnected = true;
                                break;
                        }
                    }
                }
            }
        }

        private async void LogInCommandExec(string provider)
        {
            InProgress = true;
            Session session = null;
            switch (provider)
            {
                case Constants.FacebookProvider:
                    if (!IsFacebookConnected)
                        session = await sessionService.LoginAsync(provider);
                    else
                        IsFacebookConnected = !applicationSettings.RemoveCredentials(provider);
                    break;
                case Constants.GoogleProvider:
                    if (!IsGoogleConnected)
                        session = await sessionService.LoginAsync(provider);
                    else
                        IsGoogleConnected = !applicationSettings.RemoveCredentials(provider);
                    break;
                case Constants.MicrosoftProvider:
                    if (!IsMicrosoftConnected)
                        session = await sessionService.LoginAsync(provider);
                    else
                        IsMicrosoftConnected = !applicationSettings.RemoveCredentials(provider);
                    break;
            }

            if (session == null)
            {
                return;
            }
            if (session.AccessToken == string.Empty)
            {
                await messageBoxService.ShowAsync("Something went wrong...", "Login Social Demo", new List<string> {"Ok"});
            }
            else
            {
                InProgress = false;
                navigationService.Navigate<MainPage>(session);
                navigationService.RemoveBackEntry();
                navigationService.RemoveBackEntry();
            }
        }

        public bool InProgress
        {
            get { return inProgress; }
            set
            {
                if (inProgress != value)
                {
                    inProgress = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsFacebookConnected
        {
            get { return isFacebookConnected; }
            set
            {
                if (isFacebookConnected != value)
                {
                    isFacebookConnected = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsGoogleConnected
        {
            get { return isGoogleConnected; }
            set
            {
                if (isGoogleConnected != value)
                {
                    isGoogleConnected = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsMicrosoftConnected
        {
            get { return isMicrosoftConnected; }
            set
            {
                if (isMicrosoftConnected != value)
                {
                    isMicrosoftConnected = value;
                    OnPropertyChanged();
                }
            }
        }
        

        #region Comannds

        public ICommand LogInCommand { get; private set; }

#if WINDOWS_APP
        public ICommand GoBackCommand { get; private set; }

        private void GoBackCommandExec()
        {
            navigationService.GoBack();
        }
#endif

        #endregion

#if WINDOWS_PHONE_APP
        public async void Continue(IActivatedEventArgs args)
        {
            var arguments = (WebAuthenticationBrokerContinuationEventArgs)args;
            WebAuthenticationResult result = arguments.WebAuthenticationResult;

            var session =  await sessionService.GetSession(result);
            navigationService.Navigate<MainPage>(session);
        }
#endif
    }
}
