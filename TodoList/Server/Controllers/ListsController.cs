using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ListsController(ITodoListsRepository todoListsRepository, IDbRepository dbRepository, IMapper mapper)
        {
            _todoListsRepository = todoListsRepository;
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<ListOfTodosDTO>> NewListOfTodos(ListOfTodosForCreationDTO listOfTodos)
        {
            try
            {
                var newListOfTodos = _mapper.Map<ListOfTodos>(listOfTodos);
                _dbRepository.Add(newListOfTodos);

                if(await _dbRepository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(GetTodoList), new { listOfTodosId = newListOfTodos.Id }, _mapper.Map<ListOfTodosDTO>(newListOfTodos));
                }
            }
            catch (Exception)
            {
               return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();
        }
        
        [HttpGet("{listOfTodosId}")]
        public async Task<ActionResult<ListOfTodosDTO>> GetTodoList(int listOfTodosId)
        {
            try
            {
                var todoList = await _todoListsRepository.GetTodoListAsync(listOfTodosId);
                if (todoList != null)
                {
                    return Ok(_mapper.Map<ListOfTodosDTO>(todoList));
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
