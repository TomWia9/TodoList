using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Server.Repositories;
using TodoList.Shared.Dto;

namespace TodoList.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly ITodoListsRepository _todoListsRepository;
        private readonly IMapper _mapper;

        public ListsController(ITodoListsRepository todoListsRepository, IMapper mapper)
        {
            _todoListsRepository = todoListsRepository;
            _mapper = mapper;
        }

        [HttpGet("{todoListId}")]
        public async Task<ActionResult<ListOfTodosDTO>> GetTodoList(int todoListId)
        {
            try
            {
                var todoList = await _todoListsRepository.GetTodoListAsync(todoListId);
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
