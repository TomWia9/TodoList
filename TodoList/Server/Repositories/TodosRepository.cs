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

        public async Task<Todo> GetTodoAsync(int listOfTodosId, int todoId)
        {
            return await _context.Todos.FirstOrDefaultAsync(t => t.ListOfTodosId == listOfTodosId && t.Id == todoId);
        }

        public async Task<IEnumerable<Todo>> GetTodosAsync(int listOfTodosId)
        {
            return await _context.Todos.Where(t => t.ListOfTodosId == listOfTodosId).ToListAsync();
        }

        public async Task<bool> ListOfTodosExists(int listOfTodosId)
        {
            return await _context.ListsOfTodos.AnyAsync(l => l.Id == listOfTodosId);
        }

        public async Task<bool> TodoExists(int listOfTodosId, string title)
        {
            return await _context.Todos.AnyAsync(l => l.ListOfTodosId == listOfTodosId && l.Title == title);
        }

        public void UpdateTodo(Todo todo)
        {
            //no code in this implementation
        }
    }
}
