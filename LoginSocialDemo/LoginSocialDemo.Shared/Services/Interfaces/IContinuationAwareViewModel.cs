using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Activation;

namespace LoginSocialDemo.Services.Interfaces
{
    public interface IContinuationAwareViewModel
    {
#if WINDOWS_PHONE_APP   
void Continue(IActivatedEventArgs args);
#endif

    }
}
