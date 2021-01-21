using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Server.Models;
using TodoList.Server.Repositories;
using TodoList.Shared.Dto;

namespace TodoList.Server.Controllers
{
    [Produces("application/json", "application/xml")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly ITodoListsRepository _todoListsRepository;
        private readonly IDbRepository _dbRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ListsController> _logger;


        public ListsController(ITodoListsRepository todoListsRepository, IDbRepository dbRepository, IMapper mapper, ILogger<ListsController> logger)
        {
            _todoListsRepository = todoListsRepository;
            _dbRepository = dbRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Create a new todo list
        /// </summary>
        /// <param name="listOfTodos">The todo list to create</param>
        /// <returns>An ActionResult of type ListOfTodosDto</returns>
        /// <response code="201">Creates and returns the created todo list</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost]
        public async Task<ActionResult<ListOfTodosDto>> NewListOfTodos(ListOfTodosForCreationDto listOfTodos)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id").Value);

                if (await _todoListsRepository.ListOfTodosExists(userId, listOfTodos.Title))
                {
                    return Conflict();
                }

                var newListOfTodos = _mapper.Map<ListOfTodos>(listOfTodos);
                newListOfTodos.UserId = userId;

                _dbRepository.Add(newListOfTodos);

                if (await _dbRepository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(GetTodoList), new { listOfTodosId = newListOfTodos.Id }, _mapper.Map<ListOfTodosDto>(newListOfTodos));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }

        /// <summary>
        /// Get todo list by id
        /// </summary>
        /// <param name="listOfTodosId">The Id of todo list you want to get</param>
        /// <returns>An ActionResult of type ListOfTodosDto</returns>
        /// <response code="200">Returns the requested todo list</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{listOfTodosId}")]
        public async Task<ActionResult<ListOfTodosDto>> GetTodoList(int listOfTodosId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id").Value);

                var todoList = await _todoListsRepository.GetTodoListAsync(userId, listOfTodosId);
                if (todoList != null)
                {
                    return Ok(_mapper.Map<ListOfTodosDto>(todoList));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return NotFound();
        }

        /// <summary>
        /// Get a list of todo lists
        /// </summary>
        /// <returns>An ActionResult of type IEnumerable</returns>
        /// <response code="200">Returns the list of todo lists</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListOfTodosDto>>> GetTodoLists()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id").Value);

                var todoLists = await _todoListsRepository.GetTodoListsAsync(userId);
                if (todoLists != null)
                {
                    return Ok(_mapper.Map<IEnumerable<ListOfTodosDto>>(todoLists));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return NotFound();
        }

        /// <summary>
        /// Update todo list
        /// </summary>
        /// <param name="listOfTodosId">The Id of todo list you want to update</param>
        /// <param name="listOfTodos">The todo list with updated values</param>
        /// <returns>An IActionResult</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPut("{listOfTodosId}")]
        public async Task<IActionResult> UpdateListOfTodo(int listOfTodosId, ListOfTodosForUpdateDto listOfTodos)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id").Value);

                if (await _todoListsRepository.ListOfTodosExists(userId, listOfTodos.Title))
                {
                    return Conflict();
                }

                var listOfTodosFromRepo = await _todoListsRepository.GetTodoListAsync(userId, listOfTodosId);

                if (listOfTodosFromRepo == null)
                {
                    return NotFound();
                }

                _mapper.Map(listOfTodos, listOfTodosFromRepo);
                _todoListsRepository.UpdateTodoList(listOfTodosFromRepo);

                if (await _dbRepository.SaveChangesAsync())
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();

        }

        /// <summary>
        /// Delete the todo list with given id
        /// </summary>
        /// <param name="listOfTodosId">The Id of todo list you want to delete</param>
        /// <returns>An IActionResult</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{listOfTodosId}")]
        public async Task<IActionResult> DeleteListOfTodos(int listOfTodosId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("id").Value);

                var listOfTodosToRemove = await _todoListsRepository.GetTodoListAsync(userId, listOfTodosId);

                if (listOfTodosToRemove == null)
                {
                    return NotFound();
                }

                _dbRepository.Remove(listOfTodosToRemove);

                if (await _dbRepository.SaveChangesAsync())
                {
                    return NoContent();
                }
            }
            catch (Exception)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();

        }
    }
}
