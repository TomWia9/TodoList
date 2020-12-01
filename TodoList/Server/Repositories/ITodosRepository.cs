using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Server.Models;

namespace TodoList.Server.Repositories
{
    public interface ITodosRepository
    {
        Task<IEnumerable<Todo>> GetTodosAsync(int listOfTodosId);
        Task<Todo> GetTodoAsync(int listOfTodosId, int todoId);
        Task<bool> ListOfTodosExists(int listOfTodosId);
        void UpdateTodo(Todo todo);

    }
}
