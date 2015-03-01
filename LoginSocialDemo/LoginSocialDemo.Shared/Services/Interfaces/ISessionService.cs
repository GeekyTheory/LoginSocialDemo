using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Security.Authentication.Web;
using LoginSocialDemo.Models;

namespace LoginSocialDemo.Services.Interfaces
{
    public interface ISessionService
    {
        Task<Session> GetSession(WebAuthenticationResult result);
        Task<Session> LoginAsync(string provider);
        void Save(Session session);
    }
}
