using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;
using Cimbalino.Toolkit.Services;
using LoginSocialDemo.Models;
using LoginSocialDemo.Services;
using LoginSocialDemo.ViewModels.Base;
using LoginSocialDemo.Views;

namespace LoginSocialDemo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private IApplicationDataService applicationSettings;
        private string facebookAccessToken;
        private string googleAccessToken;
        private string microsoftAccessToken;
        private List<Session> sessions; 

        public MainViewModel(INavigationService navigationService, IApplicationDataService applicationSettings)
        {
            this.navigationService = navigationService;
            this.applicationSettings = applicationSettings;

            sessions = new List<Session>();

            LogInCommand = new DelegateCommand(LogInCommandExec);

#if WINDOWS_APP
            GoBackCommand = new DelegateCommand(GoBackCommandExec, GoBackCommandCanExec);
#endif

        }

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Session)
            {
                var session = e.Parameter as Session;
                if (session != null)
                {
                    switch (session.Provider)
                    {
                        case Constants.FacebookProvider:
                            FacebookAccessToken = session.AccessToken;
                            break;
                        case Constants.GoogleProvider:
                            GoogleAccessToken = session.AccessToken;
                            break;
                        case Constants.MicrosoftProvider:
                            MicrosoftAccessToken = session.AccessToken;
                            break;
                    }
                    
                }
            }

            try
            {
                sessions = applicationSettings.LoadCredential();

                foreach (var session in sessions)
                {
                    switch (session.Provider)
                    {
                        case Constants.FacebookProvider:
                            FacebookAccessToken = session.AccessToken;
                            break;
                        case Constants.GoogleProvider:
                            GoogleAccessToken = session.AccessToken;
                            break;
                        case Constants.MicrosoftProvider:
                            MicrosoftAccessToken = session.AccessToken;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }

        #region Properties

        public string FacebookAccessToken
        {
            get { return facebookAccessToken; }
            set
            {
                if (facebookAccessToken != value)
                {
                    facebookAccessToken = value;
                    OnPropertyChanged();
                }
            }
        }

        public string GoogleAccessToken
        {
            get { return googleAccessToken; }
            set
            {
                if (googleAccessToken != value)
                {
                    googleAccessToken = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MicrosoftAccessToken
        {
            get { return microsoftAccessToken; }
            set
            {
                if (microsoftAccessToken != value)
                {
                    microsoftAccessToken = value;
                    OnPropertyChanged();
                }
            }
        }
        
        
        
        #endregion

        #region Commands

        public ICommand LogInCommand { get; private set; }

        private void LogInCommandExec()
        {
            navigationService.Navigate<LogInView>(sessions);
        }

#if WINDOWS_APP
        public ICommand GoBackCommand { get; private set; }


        private bool GoBackCommandCanExec()
        {
            if (navigationService.CanGoBack)
                return true;
            return false;
        }

        private void GoBackCommandExec()
        {
            navigationService.GoBack();
        }
#endif


        #endregion
    }
}
