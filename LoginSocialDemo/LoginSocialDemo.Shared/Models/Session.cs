using System;
using System.Collections.Generic;
using System.Text;

namespace LoginSocialDemo.Models
{
    public class Session
    {
        public string AccessToken { get; set; }
        public string Id { get; set; }
        public int ExpireDate { get; set; }
        public string Provider { get; set; }
    }
}
