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

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected ListOfTodosDto ListOfTodos { get; set; }
        protected int NumberOfIncompletedTodos { get; set; } = 0;
        protected bool LoadFailed { get; set; }
        protected bool UpdateFailed { get; set; }

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

        protected async Task UpdateStatus(TodoDto todo)
        {
            try
            {
                todo.IsDone = !todo.IsDone;

                var response = await HttpClient.PutAsJsonAsync(
                    $"api/lists/{todo.ListOfTodosId}/Todos/{todo.Id}",
                    new TodoForUpdateDto() { Title = todo.Title, Description = todo.Description, IsDone = todo.IsDone });

                UpdateFailed = !response.IsSuccessStatusCode;

                await GetListOfTodos();

            }
            catch
            {
                UpdateFailed = true;
            }
        }

        protected async Task GetListOfTodos()
        {
            try
            {
                ListOfTodos = await HttpClient.GetFromJsonAsync<ListOfTodosDto>($"api/lists/{ListId}");
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
