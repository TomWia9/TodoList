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

        private readonly HttpClient _http;

        public AppState(HttpClient http)
        {
            _http = http;
        }

        public async Task ReloadLists()
        {
            ListsOfTodos = await _http.GetFromJsonAsync<IEnumerable<ListOfTodosDto>>("api/lists");
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnNewListCreated?.Invoke();

        //public async Task<ListOfTodosDto> GetListOfTodos(int listId)
        //{
        //    return ListsOfTodos.FirstOrDefault(l => l.Id == listId);
        //}
    }
}
