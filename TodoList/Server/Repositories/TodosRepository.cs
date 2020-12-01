using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Server.Models;

namespace TodoList.Server.Repositories
{
    public class TodosRepository : ITodosRepository
    {
        private readonly TodoContext _context;

        public TodosRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<Todo> GetTodoAsync(int todoId)
        {
            return await _context.Todos.FindAsync(todoId);
        }

        public async Task<IEnumerable<Todo>> GetTodosAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public void UpdateTodo(Todo todo)
        {
            //no code in this implementation
        }
    }
}
