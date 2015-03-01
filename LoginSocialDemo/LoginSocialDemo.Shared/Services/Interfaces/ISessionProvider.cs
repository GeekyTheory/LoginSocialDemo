using System.Threading.Tasks;
using LoginSocialDemo.Models;

namespace LoginSocialDemo.Services.Interfaces
{
    public interface ISessionProvider
    {
        Task<Session> LoginAsync();
    }
}
