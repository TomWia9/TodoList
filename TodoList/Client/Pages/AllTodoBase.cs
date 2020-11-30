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
    public partial class AllTodoBase : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }
        public IEnumerable<TodoDTO> Todos { get; set; } = new List<TodoDTO>();

        protected override async Task OnInitializedAsync()
        {
            Todos = await HttpClient.GetFromJsonAsync<IEnumerable<TodoDTO>>("api/todo");
        }
    }
}
