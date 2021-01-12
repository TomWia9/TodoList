using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoList.Client.Services;
using TodoList.Client.Shared;
using TodoList.Shared.Dto;

namespace TodoList.Client.Pages
{
    public class AllTodoBase : ComponentBase
    {
        [Inject]
        protected AppStateContainer AppState { get; set; }
        public IEnumerable<int> TodoListsIds { get; set; } = new List<int>();
        protected int NumberOfAllIncompletedTodos { get; set; }

        protected override void OnInitialized()
        {
            TodoListsIds = AppState.ListsOfTodos.Select(l => l.Id).ToList();
            GetNumberOfAllIncompletedTodosAsync();
        }

        protected void GetNumberOfAllIncompletedTodosAsync()
        {
                if (AppState.NumberOfAllIncompletedTodos != null)
                    NumberOfAllIncompletedTodos = (int) AppState.NumberOfAllIncompletedTodos;
        }
    }
}
