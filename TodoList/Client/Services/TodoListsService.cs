using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoList.Shared.Dto;

namespace TodoList.Client.Services
{
    public class TodoListsService
    {
        public IEnumerable<ListOfTodosDto> ListsOfTodos = new List<ListOfTodosDto>();

        public event Action OnNewListCreated;

        private readonly HttpClient _http;

        public TodoListsService(HttpClient http)
        {
            _http = http;
        }

        public async Task GetAllListsOfTodos()
        {
            ListsOfTodos = await _http.GetFromJsonAsync<IEnumerable<ListOfTodosDto>>("api/lists");
            NotifyStateChanged();
        }

        public ListOfTodosDto GetListOfTodos(int listId)
        {
            return ListsOfTodos.FirstOrDefault(l => l.Id == listId);
        }

        private void NotifyStateChanged() => OnNewListCreated?.Invoke();

        
    }
}
