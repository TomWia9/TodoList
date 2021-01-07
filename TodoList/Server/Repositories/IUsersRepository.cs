using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Server.Models;
using TodoList.Shared.Auth;

namespace TodoList.Server.Repositories
{
    public interface IUsersRepository
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<bool> IsUsernameTaken(string userName);
        Task<User> GetUserById(int id);
    }
}
