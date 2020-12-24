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
        private readonly HttpClient _http;

        public TodoListsService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IEnumerable<ListOfTodosDto>> GetAllListsOfTodos()
        {
            return await _http.GetFromJsonAsync<IEnumerable<ListOfTodosDto>>("api/lists");
        }

        public async Task<ListOfTodosDto> GetListOfTodos(int listId)
        {
            return await _http.GetFromJsonAsync<ListOfTodosDto>($"api/lists/{listId}");
        }

        public async Task<int> GetNumberOfIncompletedTodos(int listId)
        {
            return await _http.GetFromJsonAsync<int>($"api/lists/{listId}/NumberOfIncompletedTodos");
        }

    }
}
