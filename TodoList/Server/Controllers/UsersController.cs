using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using TodoList.Server.Models;
using TodoList.Server.Repositories;
using TodoList.Shared.Auth;
using TodoList.Shared.Dto;
using TodoList.Server.Helpers;

namespace TodoList.Server.Controllers
{
    [Produces("application/json", "application/xml")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _userRepository;
        private readonly IDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public UsersController(IUsersRepository userRepository, IMapper mapper, IDbRepository dbRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _dbRepository = dbRepository;
        }

        /// <summary>
        /// Authenticate the user
        /// </summary>
        /// <param name="authenticateRequest">Username and password of user</param>
        /// <returns>An ActionResult of type AuthenticateResponse</returns>
        /// <response code="200">Returns the user with token</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticateResponse>> Authenticate(AuthenticateRequest authenticateRequest)
        {
            try
            {
                var response = await _userRepository.Authenticate(authenticateRequest);

                if (response == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user">User to create</param>
        /// <returns>An ActionResult of type UserDto</returns>
        ///<response code="201">Creates and returns created user</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UserDto>> Register(UserForCreationDto user)
        {
            try
            {
                if (await _userRepository.IsUsernameTaken(user.Username))
                {
                    return Conflict();
                }

                var newUser = _mapper.Map<User>(user);
                newUser.Password = Hash.GetHash(user.Password);

                _dbRepository.Add(newUser);

                if (await _dbRepository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(GetUser), new { userId = newUser.Id }, _mapper.Map<UserDto>(newUser));
                }


            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="userId">The id of user you want to get</param>
        /// <returns>An ActionResult of type UserDto></returns>
        /// <response code="200">Returns the requested user</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult<UserDto>> GetUser(int userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                if (user != null)
                {
                    return Ok(_mapper.Map<UserDto>(user));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return NotFound();
        }
    }
}
