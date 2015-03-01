using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Cimbalino.Toolkit.Extensions;
using LoginSocialDemo.Models;

namespace LoginSocialDemo.Services
{
    public interface IApplicationDataService
    {
        void SaveCredential(string provider, int expireDate, string accessToken);
        List<Session> LoadCredential();
        bool RemoveCredentials(string provider);
        IPropertySet LocalSettings { get; }
        IPropertySet RoamingSettings { get; }
    }

    public class ApplicationDataService : IApplicationDataService
    {
        public void SaveCredential(string provider, int expireDate, string accessToken)
        {
            var vault = new PasswordVault();
            vault.Add(new PasswordCredential(provider, expireDate.ToString(), accessToken));
        }

        public List<Session> LoadCredential()
        {
            var vault = new PasswordVault();
            List<Session> sessions = new List<Session>();
            var listCredentials = vault.RetrieveAll();
            foreach (PasswordCredential credential in listCredentials)
            {
                if (credential != null)
                {
                    credential.RetrievePassword();
                    sessions.Add(new Session()
                    {
                        AccessToken = credential.Password,
                        Id = credential.UserName,
                        Provider = credential.Resource,
                    });
                }
            }

            return sessions;
        }

        public bool RemoveCredentials(string provider)
        {
            try
            {
                var vault = new PasswordVault();
                vault.Remove(vault.FindAllByResource(provider).FirstOrDefault());
                return true;
            }
            catch (Exception)
            {
                
            }
            return false;
        }

        public IPropertySet LocalSettings {
            get
            {
                return Windows.Storage.ApplicationData.Current.LocalSettings.Values;
            } }
        public IPropertySet RoamingSettings { 
            get
            {
                return Windows.Storage.ApplicationData.Current.RoamingSettings.Values;
            } }
    }
}
