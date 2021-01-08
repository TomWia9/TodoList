using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Shared.Auth;

namespace TodoList.Client.Services
{
    public interface IAuthenticationService
    {
        AuthenticateResponse User { get; }
        Task Initialize();
        Task Login(string username, string password);
        Task Logout();
    }
}
