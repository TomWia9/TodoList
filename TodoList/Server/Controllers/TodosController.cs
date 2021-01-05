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

        [HttpGet("{todoId}")]
        public async Task<ActionResult<IEnumerable>> GetTodoOfList(int listOfTodosId, int todoId)
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

                //// Trigger validation manually
                //var validationResult = await new TodoForUpdateValidator().ValidateAsync(todoToPatch);
                //if (!validationResult.IsValid)
                //{
                //    // Add validation errors to ModelState
                //    foreach (var error in validationResult.Errors)
                //    {
                //        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                //    }

                //    // Patch failed, return 400 result
                //    return BadRequest(ModelState);
                //}

                if (await _todoRepository.TodoExists(listOfTodosId, todoToPatch.Title))
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
