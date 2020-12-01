using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Server.Models;

namespace TodoList.Server.Repositories
{
    public interface ITodoListsRepository
    {
        Task<IEnumerable<ListOfTodos>> GetTodoListsAsync();
        Task<ListOfTodos> GetTodoListAsync(int todoListId);
        void UpdateTodoList(ListOfTodos listOfTodos);
    }
}
