using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Server.Models;
using TodoList.Server.Repositories;
using TodoList.Shared.Dto;

namespace TodoList.Server.Controllers
{
    [Produces("application/json", "application/xml")]
    //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
    //[Authorize]
    [Route("api/lists/{listOfTodosId}/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodosRepository _todoRepository;
        private readonly IDbRepository _dbRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TodosController> _logger;

        public TodosController(ITodosRepository todoRepository, IDbRepository dbRepository, IMapper mapper, ILogger<TodosController> logger)
        {
            _todoRepository = todoRepository;
            _dbRepository = dbRepository;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Create a new todo
        /// </summary>
        /// <param name="listOfTodosId">The todo list id for which to create todo</param>
        /// <param name="todo">The todo to create</param>
        /// <returns>An ActionResult of type TodoDto</returns>
        /// <response code="201">Creates and returns the created todo</response>
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [HttpPost]
        public async Task<ActionResult<TodoDto>> NewTodo(int listOfTodosId, TodoForCreationDto todo)
        {
            try
            {
                if (!await _todoRepository.ListOfTodosExists(listOfTodosId))
                {
                    return NotFound();
                }

                if (await _todoRepository.TodoExists(listOfTodosId, todo.Title))
                {
                    return Conflict();
                }

                var newTodo = _mapper.Map<Todo>(todo);
                newTodo.ListOfTodosId = listOfTodosId;
                _dbRepository.Add(newTodo);

                if (await _dbRepository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(GetTodoOfList),
                        new { listOfTodosId, todoId = newTodo.Id}, _mapper.Map<TodoDto>(newTodo));
                }
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();

        }

        /// <summary>
        /// Get a list of todos from specified todo list
        /// </summary>
        /// <param name="listOfTodosId">The Id of todo list you want to get todos from</param>
        /// <returns>An ActionResult of type IEnumerable</returns>
        /// <response code="200">Returns the requested list of todos from specified todo list</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetTodosOfList(int listOfTodosId)
        {
            try
            {
                if (!await _todoRepository.ListOfTodosExists(listOfTodosId))
                {
                    return NotFound();
                }
                var todos = await _todoRepository.GetTodosAsync(listOfTodosId);
                if (todos != null)
                {
                    return Ok(_mapper.Map<IEnumerable<TodoDto>>(todos));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return NotFound();
        }

        /// <summary>
        /// Get todo from specified todo list
        /// </summary>
        /// <param name="listOfTodosId">The Id of todo list you want to get todo from</param>
        /// <param name="todoId">The Id of todo you want to get</param>
        /// <returns>An ActionResult of type TodoDto</returns>
        /// <response code="200">Returns the requested todo from specified todo list</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{todoId}")]
        public async Task<ActionResult<TodoDto>> GetTodoOfList(int listOfTodosId, int todoId)
        {
            try
            {
                if (!await _todoRepository.ListOfTodosExists(listOfTodosId))
                {
                    return NotFound();
                }

                var todo = await _todoRepository.GetTodoAsync(listOfTodosId, todoId);
                if (todo != null)
                {
                    return Ok(_mapper.Map<TodoDto>(todo));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return NotFound();
        }

        /// <summary>
        /// Update todo
        /// </summary>
        /// <param name="listOfTodosId">The Id of todo list which you want to update todo</param>
        /// <param name="todoId">The Id of todo you want to update</param>
        /// <param name="todo">The todo with updated values</param>
        /// <returns>An IActionResult</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut("{todoId}")]
        public async Task<IActionResult> UpdateTodo(int listOfTodosId, int todoId, TodoForUpdateDto todo)
        {
            try
            {

                if (!await _todoRepository.ListOfTodosExists(listOfTodosId))
                {
                    return NotFound();
                }

                if (await _todoRepository.TodoExists(listOfTodosId, todo.Title, todoId))
                {
                    return Conflict();
                }

                var todoFromRepo = await _todoRepository.GetTodoAsync(listOfTodosId, todoId);

                if (todoFromRepo == null)
                {
                    return NotFound();
                }

                _mapper.Map(todo, todoFromRepo);
                _todoRepository.UpdateTodo(todoFromRepo);

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
        /// Partially update a todo
        /// </summary>
        /// <param name="listOfTodosId">The Id of todo list which you want to partially update todo</param>
        /// <param name="todoId">The Id of todo you want to partially update</param>
        /// <param name="patchDocument">The set of operations to apply to the todo</param>
        /// <returns>An IActionResult</returns>
        /// <remarks>
        /// Sample request (this request updates the todos's **isDone** status)
        ///
        ///     PATCH /lists/listId/todos/todoId 
        ///     [ 
        ///         { 
        ///             "op": "replace", 
        ///             "patch": "/isDone", 
        ///             "value": true 
        ///         } 
        ///     ]
        /// </remarks>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPatch("{todoId}")]
        public async Task<IActionResult> PartiallyUpdateTodo(int listOfTodosId, int todoId, JsonPatchDocument<TodoForUpdateDto> patchDocument)
        {
            try
            {
                if (!await _todoRepository.ListOfTodosExists(listOfTodosId))
                {
                    return NotFound();
                }

                var todoFromRepo = await _todoRepository.GetTodoAsync(listOfTodosId, todoId);

                if (todoFromRepo == null)
                {
                    return NotFound();
                }

                var todoToPatch = _mapper.Map<TodoForUpdateDto>(todoFromRepo);
                patchDocument.ApplyTo(todoToPatch, ModelState);

                //check if todo with new title exists (there cannot be 2 todos with the same title)
                if (await _todoRepository.TodoExists(listOfTodosId, todoToPatch.Title, todoId))
                {
                    return Conflict();
                }

                _mapper.Map(todoToPatch, todoFromRepo);
                _todoRepository.UpdateTodo(todoFromRepo);

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
        /// Delete the todo with given id
        /// </summary>
        /// <param name="listOfTodosId">The Id of todo list you want to delete todo from</param>
        /// <param name="todoId">The id of todo you want to delete</param>
        /// <returns>An IActionResult</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{todoId}")]
        public async Task<IActionResult> DeleteTodo(int listOfTodosId, int todoId)
        {
            try
            {
                if (! await _todoRepository.ListOfTodosExists(listOfTodosId))
                {
                    return NotFound();
                }

                var todoToRemove = await _todoRepository.GetTodoAsync(listOfTodosId, todoId);

                if(todoToRemove == null)
                {
                    return NotFound();
                }

                _dbRepository.Remove(todoToRemove);

                if(await _dbRepository.SaveChangesAsync())
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
