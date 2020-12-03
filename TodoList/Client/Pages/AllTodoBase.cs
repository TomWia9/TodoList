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
        public IEnumerable<ListOfTodosDto> ListOfTodos { get; set; } = new List<ListOfTodosDto>();
        public int NumberOfIncompletedTodos { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            ListOfTodos = await HttpClient.GetFromJsonAsync<IEnumerable<ListOfTodosDto>>("api/lists");

            if (ListOfTodos != null)
            {
                foreach (var list in ListOfTodos)
                {
                    NumberOfIncompletedTodos += list.Todos.Count(t => !t.IsDone);
                }
            }
              
        }
    }
}
