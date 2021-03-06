﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using LoginSocialDemo.Models;


namespace LoginSocialDemo.Services.Interfaces
{
    public interface IGoogleService : ISessionProvider
    {
        Task<Session> GetSessionAsync(WebAuthenticationResult result);
    }
}
