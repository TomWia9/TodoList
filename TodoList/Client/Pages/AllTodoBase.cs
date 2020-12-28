using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoList.Client.Services;
using TodoList.Shared.Dto;

namespace TodoList.Client.Pages
{
    public class AllTodoBase : ComponentBase
    {
        [Inject]
        protected  TodoListsService TodoListsService {get; set; }
        
        public IEnumerable<int> TodoListsIds { get; set; } = new List<int>();
        protected int NumberOfAllIncompletedTodos { get; set; }

        protected override async Task OnInitializedAsync()
        {
            TodoListsIds = await TodoListsService.GetTodoListsIdsAsync();
            NumberOfAllIncompletedTodos = await TodoListsService.GetNumberOfAllIncompletedTodosAsync();
        }
    }
}
