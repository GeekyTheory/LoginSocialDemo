using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Autofac.Core;
using Cimbalino.Toolkit.Services;
using LoginSocialDemo.Services;
using LoginSocialDemo.Services.Interfaces;

namespace LoginSocialDemo.ViewModels.Base
{
    public class ViewModelLocator
    {
        private IContainer container;
        public ViewModelLocator()
        {
            ContainerBuilder builder = new ContainerBuilder();

            //Register Instances
            builder.RegisterType<NavigationService>().As<INavigationService>();

            builder.RegisterType<NetworkInformationService>().As<INetworkInformationService>();
            builder.RegisterType<ApplicationDataService>().As<IApplicationDataService>();
            builder.RegisterType<MessageBoxService>().As<IMessageBoxService>();
            builder.RegisterType<SessionService>().As<ISessionService>();
            builder.RegisterType<FacebookService>().As<IFacebookService>();
            builder.RegisterType<GoogleService>().As<IGoogleService>();
            builder.RegisterType<MicrosoftService>().As<IMicrosoftService>();

            builder.RegisterType<MainViewModel>();
            builder.RegisterType<LogInViewModel>();

            this.container = builder.Build();
        }

        public MainViewModel MainViewModel { get { return this.container.Resolve<MainViewModel>(); } }
        public LogInViewModel LogInViewModel { get { return this.container.Resolve<LogInViewModel>(); } }
    }
}
