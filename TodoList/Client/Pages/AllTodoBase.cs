using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoList.Shared.Dto;

namespace TodoList.Client.Pages
{
    public class AllTodoBase : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }
        public IEnumerable<int> TodoListsIds { get; set; } = new List<int>();

        protected override async Task OnInitializedAsync()
        {
            await GetTodoListsIdsAsync();
        }

        private async Task GetTodoListsIdsAsync()
        {
            TodoListsIds = await HttpClient.GetFromJsonAsync<IEnumerable<int>>("api/lists/ids");
        }
    }
}
