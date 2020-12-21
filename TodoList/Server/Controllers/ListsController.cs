using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Server.Models;
using TodoList.Server.Repositories;
using TodoList.Shared.Dto;

namespace TodoList.Server.Controllers
{
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

        [HttpPost]
        public async Task<ActionResult<ListOfTodosDto>> NewListOfTodos(ListOfTodosForCreationDto listOfTodos)
        {
            try
            {
                if (await _todoListsRepository.ListOfTodosExists(listOfTodos.Title))
                {
                    return Conflict();
                }

                var newListOfTodos = _mapper.Map<ListOfTodos>(listOfTodos);
                _dbRepository.Add(newListOfTodos);

                if(await _dbRepository.SaveChangesAsync())
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
        
        [HttpGet("{listOfTodosId}")]
        public async Task<ActionResult<ListOfTodosDto>> GetTodoList(int listOfTodosId)
        {
            try
            {
                var todoList = await _todoListsRepository.GetTodoListAsync(listOfTodosId);
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListOfTodosDto>>> GetTodoLists()
        {
            try
            {
                var todoLists = await _todoListsRepository.GetTodoListsAsync();
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

        [HttpGet("ids")]
        public async Task<ActionResult<IEnumerable<ListOfTodosDto>>> GetTodoListsIds()
        {
            try
            {
                var todoListsIds = await _todoListsRepository.GetTodoListsIdsAsync();
                if (todoListsIds != null)
                {
                    return Ok(todoListsIds);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return NotFound();
        }


        [HttpGet("{listOfTodosId}/NumberOfIncompletedTodos")]
        public async Task<ActionResult<int>> GetNumberOfIncompletedTodos(int listOfTodosId)
        {
            try
            {
                if (!await _todoListsRepository.ListOfTodosExists(listOfTodosId))
                {
                    return NotFound();
                }

                var numberOfIncompletedTodos = await _todoListsRepository.GetNumberOfIncompletedTodos(listOfTodosId);
                return Ok(numberOfIncompletedTodos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

        }

        [HttpGet("NumberOfAllIncompletedTodos")]
        public async Task<ActionResult<int>> GetNumberOfAllIncompletedTodos()
        {
            try
            {
                var numberOfAllIncompletedTodos = await _todoListsRepository.GetNumberOfAllIncompletedTodos();
                return Ok(numberOfAllIncompletedTodos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

        }

        [HttpPut("{listOfTodosId}")]
        public async Task<IActionResult> UpdateTodo(int listOfTodosId, ListOfTodosForUpdateDto listOfTodos)
        {
            try
            {
                if (await _todoListsRepository.ListOfTodosExists(listOfTodos.Title))
                {
                    return Conflict();
                }

                var listOfTodosFromRepo = await _todoListsRepository.GetTodoListAsync(listOfTodosId);

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

        [HttpDelete("{listOfTodosId}")]
        public async Task<IActionResult> DeleteListOfTodos(int listOfTodosId)
        {
            try
            {
                var listOfTodosToRemove = await _todoListsRepository.GetTodoListAsync(listOfTodosId);

                if(listOfTodosToRemove == null)
                {
                    return NotFound();
                }

                _dbRepository.Remove(listOfTodosToRemove);

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
