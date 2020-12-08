using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using TodoList.Shared.Dto;

namespace TodoList.Client.Pages
{
    public class ListOfTodosBase : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }
        [Inject]
        protected  NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int Id { get; set; }
        public ListOfTodosDto ListOfTodos { get; set; }
        public int NumberOfIncompletedTodos { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            await GetListOfTodos();
        }

        protected override async Task OnParametersSetAsync()
        {
            await GetListOfTodos();
        }

        private async Task GetListOfTodos()
        {
            ListOfTodos = await HttpClient.GetFromJsonAsync<ListOfTodosDto>($"api/lists/{Id}");

            if (ListOfTodos != null)
            {
                NumberOfIncompletedTodos = ListOfTodos.Todos.Count(t => !t.IsDone);
            }
        }
    }
}
