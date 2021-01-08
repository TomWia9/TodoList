using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Shared.Dto;

namespace TodoList.Client.Services
{
    public interface IUsersService
    {
        Task<UserDto> GetUser(int userId);

    }
}
