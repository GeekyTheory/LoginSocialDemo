using System;
using System.Collections.Generic;
using System.Text;

namespace LoginSocialDemo
{
    public class Constants
    {
        /// <summary> 
        /// The google callback url. 
        /// </summary> 
#if !WINDOWS_PHONE_APP
        public const string GoogleCallbackUrl = "urn:ietf:wg:oauth:2.0:oob";
#else
        public const string GoogleCallbackUrl = "http://localhost";
#endif

        public const string FacebookAppId = "531359217008502";

        public const string GoogleClientId = "267117539715-bn9l455efp4ocgu5b08nibju7beah0c3.apps.googleusercontent.com";

        public const string GoogleClientSecret = "TARwpLLm5PUvL2dSG02ub76i";

        public const string LoginToken = "LoginToken";

        public const string FacebookProvider = "facebook";

        public const string GoogleProvider = "google";

        public const string MicrosoftProvider = "microsoft";
    }
}
