using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoList.Shared.Dto;

namespace TodoList.Client.Shared
{
    public class AppState
    {
        public IEnumerable<ListOfTodosDto> ListsOfTodos = new List<ListOfTodosDto>();

        public event Action OnNewListCreated;

        public void ReloadLists(IEnumerable<ListOfTodosDto> listOfTodos)
        {
            ListsOfTodos = listOfTodos;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnNewListCreated?.Invoke();
    }
}
