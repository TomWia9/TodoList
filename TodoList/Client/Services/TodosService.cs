using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoList.Shared.Dto;
using TodoList.Client.Helpers.ExtensionMethods;
using TodoList.Shared.Auth;

namespace TodoList.Client.Services
{
    public class TodosService
    {

        private readonly IHttpService _httpService;

        public TodosService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<HttpResponseMessage> UpdateStatus(TodoDto todo)
        {
            var patchDocument = new JsonPatchDocument<TodoForUpdateDto>().Replace(o => o.IsDone, !todo.IsDone);

            //not sure that will work because patchDocument and extension PatchAsync
            return await _httpService.Patch($"api/lists/{todo.ListOfTodosId}/Todos/{todo.Id}", patchDocument);
            
            //return await _http.PatchAsync($"api/lists/{todo.ListOfTodosId}/Todos/{todo.Id}",
            //    patchDocument);
        }

        public async Task<HttpResponseMessage> CreateTodo(int listId, TodoForCreationDto todo)
        {
            return await _httpService.Post($"api/lists/{listId}/todos", todo);
        }

        public async Task<HttpResponseMessage> UpdateTodo(int listId, int todoId, TodoForUpdateDto todo)
        {
            return await _httpService.Put($"api/lists/{listId}/Todos/{todoId}", todo);
        }

        public async Task<HttpResponseMessage> DeleteTodo(int listId, int todoId)
        {
            return await _httpService.Delete($"api/lists/{listId}/todos/{todoId}");
        }
    }
}
