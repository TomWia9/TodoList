using System.Net.Http;
using System.Threading.Tasks;
using TodoList.Shared.Auth;
using TodoList.Shared.Dto;

namespace TodoList.Client.Services
{
    public interface IAuthenticationService
    {
        AuthenticateResponse User { get; }
        Task Initialize();
        Task Login(string username, string password);
        Task Logout();
        Task<HttpResponseMessage> Register(UserForCreationDto user);
    }
}
