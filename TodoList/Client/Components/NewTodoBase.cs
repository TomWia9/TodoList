using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TodoList.Client.Services;
using TodoList.Client.Shared;
using TodoList.Shared.Dto;

namespace TodoList.Client.Components
{
    public class NewTodoBase : ComponentBase
    {
        [Inject]
        protected ITodosService TodosService { get; set; }

        [Inject]
        protected AppStateContainer AppState { get; set; }

        [Parameter]
        public int ListId { get; set; }

        [Parameter] 
        public EventCallback OnCreated { get; set; }

        protected TodoForCreationDto TodoForCreation { get; set; } = new();
        protected  bool TodoAlreadyExists { get; set; }
        protected bool CreationFailed { get; set; }

        protected override void OnParametersSet()
        {
            TodoForCreation = new TodoForCreationDto();
            CreationFailed = false;
            TodoAlreadyExists = false;
        }

        protected async Task CreateTodo()
        {
            var response = await TodosService.CreateTodo(ListId, TodoForCreation);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                TodoAlreadyExists = true;
            }
            else if (!response.IsSuccessStatusCode)
            {
                CreationFailed = true;
            }
            else
            { 
                CreationFailed = false;
                var todo = await response.Content.ReadFromJsonAsync<TodoDto>();
                AppState.AddTodo(todo);
               await OnCreated.InvokeAsync();
            }
        }
    }
}
