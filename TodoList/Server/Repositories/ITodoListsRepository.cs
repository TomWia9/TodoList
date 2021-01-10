using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Server.Models;

namespace TodoList.Server.Repositories
{
    public interface ITodoListsRepository
    {
        Task<IEnumerable<ListOfTodos>> GetTodoListsAsync(int userId);
        Task<ListOfTodos> GetTodoListAsync(int todoListId, int userId);
        Task<bool> ListOfTodosExists(string title);
        Task<bool> ListOfTodosExists(int id);
        void UpdateTodoList(ListOfTodos listOfTodos);
        Task<int> GetNumberOfIncompletedTodos(int listOfTodosId);
        Task<int> GetNumberOfAllIncompletedTodos();
    }
}
