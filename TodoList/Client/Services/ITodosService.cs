using System.Net.Http;
using System.Threading.Tasks;
using TodoList.Shared.Dto;

namespace TodoList.Client.Services
{
    public interface ITodosService
    {
        Task<HttpResponseMessage> UpdateStatus(TodoDto todo);
        Task<HttpResponseMessage> CreateTodo(int listId, TodoForCreationDto todo);
        Task<HttpResponseMessage> UpdateTodo(int listId, int todoId, TodoForUpdateDto todo);
        Task<HttpResponseMessage> DeleteTodo(int listId, int todoId);
    }
}