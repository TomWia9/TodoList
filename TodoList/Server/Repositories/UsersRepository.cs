using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoList.Server.Helpers;
using TodoList.Server.Models;
using TodoList.Shared.Auth;
using TodoList.Shared.Dto;

namespace TodoList.Server.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly TodoContext _context;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;

        public UsersRepository(TodoContext context, IOptions<AppSettings> appSettings, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest authenticateRequest)
        {
            var userFromDb = await _context.Users.SingleOrDefaultAsync(u => u.Username == authenticateRequest.Username && u.Password == Hash.GetHash(authenticateRequest.Password));

            if (userFromDb == null)
            {
                return null;
            }

            var user = _mapper.Map<UserDto>(userFromDb);

            // authentication successful so generate AuthenticateResponse with jwt token
            return new AuthenticateResponse()
            {
                Id = user.Id,
                Username = user.Username,
                Token = Token.GenerateToken(user, _appSettings.Secret)
            };
        }

        public async Task<bool> IsUsernameTaken(string userName)
        {
            return await _context.Users.AnyAsync(u => u.Username == userName);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
