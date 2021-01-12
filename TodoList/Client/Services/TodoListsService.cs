using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoList.Client.Helpers.ExtensionMethods;
using TodoList.Shared.Auth;
using TodoList.Shared.Dto;

namespace TodoList.Client.Services
{
    public class TodoListsService : ITodoListsService
    {
        private readonly IHttpService _httpService;

        public TodoListsService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IEnumerable<ListOfTodosDto>> GetAllListsOfTodosAsync()
        {
            var response = await _httpService.Get("api/lists");
            return await response.Content.ReadFromJsonAsync<IEnumerable<ListOfTodosDto>>();
        }

        public async Task<HttpResponseMessage> GetListOfTodosAsync(int listId)
        {
            return await _httpService.Get($"api/lists/{listId}");
        }

        public async Task<HttpResponseMessage> UpdateList(int listId, ListOfTodosForUpdateDto listOfTodos)
        {
            return await _httpService.Put($"api/lists/{listId}", listOfTodos);
        }

        public async Task<HttpResponseMessage> CreateList(ListOfTodosForCreationDto listOfTodos)
        {
            return await _httpService.Post("api/lists", listOfTodos);
        }

        public async Task<HttpResponseMessage> DeleteList(int listId)
        {
            return await _httpService.Delete($"api/lists/{listId}");
        }

    }
}
