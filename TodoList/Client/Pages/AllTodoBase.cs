using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using TodoList.Client.Shared;

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
                NumberOfAllIncompletedTodos = (int)AppState.NumberOfAllIncompletedTodos;
        }
    }
}
