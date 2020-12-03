using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Server.Models;

namespace TodoList.Server.Repositories
{
    public class TodoListsRepository : ITodoListsRepository
    {
        private readonly TodoContext _context;

        public TodoListsRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<ListOfTodos> GetTodoListAsync(int todoListId)
        {
            return await _context.ListsOfTodos.Include(l => l.Todos).FirstOrDefaultAsync(l => l.Id == todoListId);
        }

        public async Task<bool> ListOfTodosExists(string title)
        {
            return await _context.ListsOfTodos.AnyAsync(t => t.Title == title);
        }

        public async Task<IEnumerable<ListOfTodos>> GetTodoListsAsync()
        {
            return await _context.ListsOfTodos.Include(l => l.Todos).ToListAsync();
        }

        public void UpdateTodoList(ListOfTodos listOfTodos)
        {
            //no code in this implementation
        }
    }
}
