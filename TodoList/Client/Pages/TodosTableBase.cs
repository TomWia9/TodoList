using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TodoList.Shared.Dto;

namespace TodoList.Client.Pages
{
    public class TodosTableBase : ComponentBase
    {
        [Parameter]
        public int ListId { get; set; }

        [Parameter]
        public EventCallback OnUpdated { get; set; }

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected ListOfTodosDto ListOfTodos { get; set; }
        protected int NumberOfIncompletedTodos { get; set; } = 0;
        protected bool LoadFailed { get; set; }
        protected bool UpdateFailed { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await GetListOfTodos();
            await GetNumberOfIncompletedTodos();
        }

        protected void NavigateToNewListComponent()
        {
            NavigationManager.NavigateTo("newList");
        }

        protected async Task UpdateStatus(TodoDto todo)
        {
            try
            {
                todo.IsDone = !todo.IsDone;

                var response = await HttpClient.PutAsJsonAsync(
                    $"api/lists/{todo.ListOfTodosId}/Todos/{todo.Id}",
                    new TodoForUpdateDto() { Title = todo.Title, Description = todo.Description, IsDone = todo.IsDone });

                UpdateFailed = !response.IsSuccessStatusCode;

                await ReloadListOfTodos();

            }
            catch
            {
                UpdateFailed = true;
            }
        }

        private async Task GetListOfTodos()
        {
            try
            {
                ListOfTodos = await HttpClient.GetFromJsonAsync<ListOfTodosDto>($"api/lists/{ListId}");

                LoadFailed = false;
            }
            catch
            {
                LoadFailed = true;
            }

        }

        private async Task GetNumberOfIncompletedTodos()
        {
            NumberOfIncompletedTodos = await HttpClient.GetFromJsonAsync<int>($"api/lists/{ListId}/NumberOfIncompletedTodos");
        }

        protected async Task ReloadListOfTodos()
        {
            await GetListOfTodos();
            await GetNumberOfIncompletedTodos();
            await OnUpdated.InvokeAsync();
        }
    }
}
