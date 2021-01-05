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

        //todoId is optional because its not necessary during creating new todo but its required during updating todo
        Task<bool> TodoExists(int listOfTodosId, string title, int? todoId = null);
        void UpdateTodo(Todo todo);

    }
}
