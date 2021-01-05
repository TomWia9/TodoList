using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoList.Shared.Dto;

namespace TodoList.Client.Services
{
    public class TodosService
    {
        private readonly HttpClient _http;

        public TodosService(HttpClient http)
        {
            _http = http;
        }

        public async Task<HttpResponseMessage> UpdateStatus(TodoDto todo)
        {
            todo.IsDone = !todo.IsDone;
            
            //this should be http.PATCH
            return await _http.PutAsJsonAsync(
                $"api/lists/{todo.ListOfTodosId}/Todos/{todo.Id}",
                new TodoForUpdateDto() { Title = todo.Title, Description = todo.Description, IsDone = todo.IsDone });
        }

        public async Task<HttpResponseMessage> CreateTodo(int listId, TodoForCreationDto todo)
        {
            return await _http.PostAsJsonAsync($"api/lists/{listId}/todos", todo);
        }

        public async Task<HttpResponseMessage> UpdateTodo(int listId, int todoId, TodoForUpdateDto todo)
        {
            return await _http.PutAsJsonAsync($"api/lists/{listId}/Todos/{todoId}", todo);
        }

        public async Task<HttpResponseMessage> DeleteTodo(int listId, int todoId)
        {
            return await _http.DeleteAsync($"api/lists/{listId}/todos/{todoId}");
        }
    }
}
