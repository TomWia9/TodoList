using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
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
        protected ListOfTodosDto ListOfTodos { get; set; }
        protected int NumberOfIncompletedTodos { get; set; } = 0;
        protected bool LoadFailed { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetListOfTodos();
        }

        protected override async Task OnParametersSetAsync()
        {
            await GetListOfTodos();
        }

        protected void NavigateToNewListComponent()
        {
            NavigationManager.NavigateTo("newList");
        }

        private async Task GetListOfTodos()
        {
            try
            {
                ListOfTodos = await HttpClient.GetFromJsonAsync<ListOfTodosDto>($"api/lists/{Id}");
                NumberOfIncompletedTodos = ListOfTodos.Todos.Count(t => !t.IsDone);

                LoadFailed = false;
            }
            catch
            {
                LoadFailed = true;
            }

        }

        
    }
}
