using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Server.Models;
using TodoList.Server.Repositories;
using TodoList.Shared.Dto;

namespace TodoList.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodosRepository _todoRepository;
        private readonly IDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public TodosController(ITodosRepository todoRepository, IDbRepository dbRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<TodoDTO>> NewTodo(TodoForCreationDTO todo)
        {
            try
            {
                var newTodo = _mapper.Map<Todo>(todo);
                _dbRepository.Add(newTodo);

                if (await _dbRepository.SaveChangesAsync())
                {
                    return CreatedAtAction(nameof(GetTodo), new { todoId = newTodo.Id }, _mapper.Map<TodoDTO>(newTodo));
                }
            }

            catch (Exception)
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

        [HttpPut("{todoId}")]
        public async Task<IActionResult> UpdateTodo(int todoId, TodoForUpdateDTO todo)
        {
            try
            {
                var todoFromRepo = await _todoRepository.GetTodoAsync(todoId);

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
        public async Task<IActionResult> PartiallyUpdateTodo(int todoId, JsonPatchDocument<TodoForUpdateDTO> patchDocument)
        {
            try
            {
                var todoFromRepo = await _todoRepository.GetTodoAsync(todoId);

                if (todoFromRepo == null)
                {
                    return NotFound();
                }

                var todoToPatch = _mapper.Map<TodoForUpdateDTO>(todoFromRepo);
                patchDocument.ApplyTo(todoToPatch, ModelState);
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
        public async Task<IActionResult> DeleteTodo(int todoId)
        {
            try
            {
                var todoToRemove = await _todoRepository.GetTodoAsync(todoId);

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
