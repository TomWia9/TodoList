using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
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
    public class TodoController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;
        private readonly IMapper _mapper;

        public TodoController(ITodoRepository todoRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<TodoDTO>> NewTodo(TodoForCreationDTO todo)
        {
            try
            {
                var newTodo = _mapper.Map<Todo>(todo);
                _todoRepository.Add(newTodo);

                if (await _todoRepository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(GetTodo), new { todoId = newTodo.Id }, _mapper.Map<TodoDTO>(newTodo));
                }
            }

            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return BadRequest();

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetTodos()
        {
            try
            {
                var todos = await _todoRepository.GetTodosAsync();
                if (todos != null)
                {
                    return Ok(_mapper.Map<IEnumerable<TodoDTO>>(todos));
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }

            return NotFound();
        }

        [HttpGet("{todoId}")]
        public async Task<ActionResult<IEnumerable>> GetTodo(int todoId)
        {
            try
            {
                var todo = await _todoRepository.GetTodoAsync(todoId);
                if (todo != null)
                {
                    return Ok(_mapper.Map<TodoDTO>(todo));
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
