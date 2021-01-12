using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TodoList.Shared.Dto;

namespace TodoList.Client.Services
{
    public interface ITodoListsService
    {
        Task<IEnumerable<ListOfTodosDto>> GetAllListsOfTodosAsync();
        Task<HttpResponseMessage> GetListOfTodosAsync(int listId);
        Task<int> GetNumberOfIncompletedTodosAsync(int listId);
        Task<int> GetNumberOfAllIncompletedTodosAsync();
        Task<HttpResponseMessage> UpdateList(int listId, ListOfTodosForUpdateDto listOfTodos);
        Task<HttpResponseMessage> CreateList(ListOfTodosForCreationDto listOfTodos);
        Task<HttpResponseMessage> DeleteList(int listId);
    }
}