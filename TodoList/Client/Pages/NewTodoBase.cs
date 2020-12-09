using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TodoList.Shared.Dto;

namespace TodoList.Client.Pages
{
    public class NewTodoBase : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Parameter]
        public int ListId { get; set; }

        protected TodoForCreationDto TodoForCreation { get; set; } = new TodoForCreationDto();

        protected async Task CreateTodo()
        {
            Console.WriteLine("CreateTodo");
            Console.WriteLine("ListId: {0}", ListId);
            //not implemented yet
        }
    }
}
