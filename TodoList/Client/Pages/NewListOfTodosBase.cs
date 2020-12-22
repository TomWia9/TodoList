using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using TodoList.Client.Shared;
using TodoList.Shared.Dto;

namespace TodoList.Client.Pages
{
    public class NewListOfTodosBase : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected AppState AppState { get; set; }

        protected ListOfTodosForCreationDto ListOfTodos { get; set; } = new ListOfTodosForCreationDto();
        protected bool CreationFailed { get; set; }
        protected bool ListAlreadyExists { get; set; }

        protected async Task CreateList()
        {
            var response = await HttpClient.PostAsJsonAsync("api/lists", ListOfTodos);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                ListAlreadyExists = true;
            }
            else if (!response.IsSuccessStatusCode)
            {
                CreationFailed = true;
            }
            else
            {
                var data = await response.Content.ReadFromJsonAsync<JsonElement>();
                var url = $"list/{data.GetProperty("id")}";
                AppState.ReloadLists(await HttpClient.GetFromJsonAsync<IEnumerable<ListOfTodosDto>>("api/lists"));
                NavigationManager.NavigateTo(url);
            }
        }
    }
}
