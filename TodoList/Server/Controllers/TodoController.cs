using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Shared.Dto;

namespace TodoList.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoDTO>>> GetAllTodos()
        {
            IList<TodoDTO> todos = new List<TodoDTO>()
            {
               new TodoDTO()
               {
                   Id = 1,
                   Title = "First ToDo",
                   Description = "Example description",
               },
               new TodoDTO()
               {
                   Id = 2,
                   Title = "Second ToDo",
                   Description = "Example second description",
                   IsDone = true
               },
               new TodoDTO()
               {
                   Id = 3,
                   Title = "Third ToDo",
                   Description = "Example third description",
               }

            };

            return Ok(todos);
        }
    }
}
