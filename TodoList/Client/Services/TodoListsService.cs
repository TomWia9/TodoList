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

        public async Task<IEnumerable<ListOfTodosDto>> GetAllListsOfTodosAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<ListOfTodosDto>>("api/lists");
        }

        public async Task<ListOfTodosDto> GetListOfTodosAsync(int listId)
        {
            return await _http.GetFromJsonAsync<ListOfTodosDto>($"api/lists/{listId}");
        }

        public async Task<int> GetNumberOfIncompletedTodosAsync(int listId)
        {
            return await _http.GetFromJsonAsync<int>($"api/lists/{listId}/NumberOfIncompletedTodos");
        }

        public async Task<int> GetNumberOfAllIncompletedTodosAsync()
        {
            return await _http.GetFromJsonAsync<int>($"api/lists/NumberOfAllIncompletedTodos");
        }
        public async Task<IEnumerable<int>> GetTodoListsIdsAsync()
        {
            return await _http.GetFromJsonAsync<IEnumerable<int>>($"api/lists/ids");
        }
        public async Task<HttpResponseMessage> CreateList(ListOfTodosForCreationDto listOfTodos)
        {
            return await _http.PostAsJsonAsync("api/lists", listOfTodos);
        }

        public async Task<HttpResponseMessage> DeleteList(int listId)
        {
            return await _http.DeleteAsync($"api/lists/{listId}");
        }

    }
}
