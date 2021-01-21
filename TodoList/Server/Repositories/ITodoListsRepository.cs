using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Server.Models;

namespace TodoList.Server.Repositories
{
    public interface ITodoListsRepository
    {
        Task<IEnumerable<ListOfTodos>> GetTodoListsAsync(int userId);
        Task<ListOfTodos> GetTodoListAsync(int userId, int todoListId);
        Task<bool> ListOfTodosExists(int userId, string title);
        Task<bool> ListOfTodosExists(int userId, int listId);
        void UpdateTodoList(ListOfTodos listOfTodos);
    }
}
