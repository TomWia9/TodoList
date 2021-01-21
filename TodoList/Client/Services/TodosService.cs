using Microsoft.AspNetCore.JsonPatch;
using System.Net.Http;
using System.Threading.Tasks;
using TodoList.Client.Shared;
using TodoList.Shared.Dto;

namespace TodoList.Client.Services
{
    public class TodosService : ITodosService
    {

        private readonly IHttpService _httpService;
        private readonly AppStateContainer _appState;

        public TodosService(IHttpService httpService, AppStateContainer appState)
        {
            _httpService = httpService;
            _appState = appState;
        }

        public async Task<HttpResponseMessage> UpdateStatus(TodoDto todo)
        {
            var patchDocument = new JsonPatchDocument<TodoForUpdateDto>().Replace(o => o.IsDone, !todo.IsDone);

            return await _httpService.Patch($"api/lists/{todo.ListOfTodosId}/Todos/{todo.Id}", patchDocument);
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
