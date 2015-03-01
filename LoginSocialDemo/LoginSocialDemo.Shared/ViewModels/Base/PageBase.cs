using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LoginSocialDemo.ViewModels.Base
{
    public class PageBase : Page
    {
        private ViewModelBase vmBase;
        public PageBase()
        {
            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            vmBase = (ViewModelBase)DataContext;
            if (vmBase != null)
            {
                vmBase.OnNavigatedTo(e);                
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            vmBase = (ViewModelBase)DataContext;
            if (vmBase != null)
            {
                vmBase.OnNavigatedFrom(e);
            }
        }
    }
}
