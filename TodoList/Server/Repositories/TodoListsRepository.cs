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

        public async Task<ListOfTodos> GetTodoListAsync(int userId, int todoListId)
        {
            var todoList = await _context.ListsOfTodos.Include(l => l.Todos).FirstOrDefaultAsync(l => l.UserId == userId && l.Id == todoListId);
            if (todoList != null)
            {
                todoList.Todos = todoList.Todos.OrderBy(t => t.IsDone).ThenByDescending(t => t.DateAdded);
            }
            
            return todoList;
        }

        public async Task<bool> ListOfTodosExists(int userId, string title)
        {
            return await _context.ListsOfTodos.AnyAsync(l => l.UserId == userId && l.Title == title);
        }

        public async Task<bool> ListOfTodosExists(int id)
        {
            return await _context.ListsOfTodos.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<ListOfTodos>> GetTodoListsAsync(int userId)
        {
            IEnumerable<ListOfTodos> todoLists = await _context.ListsOfTodos.Include(l => l.Todos).Where(l => l.UserId == userId).ToListAsync();

            foreach (var todoList in todoLists)
            {
                todoList.Todos = todoList.Todos.OrderBy(t => t.IsDone).ThenByDescending(t => t.DateAdded);
            }

            return todoLists;
        }

        public void UpdateTodoList(ListOfTodos listOfTodos)
        {
            //no code in this implementation
        }

        public async Task<int> GetNumberOfIncompletedTodos(int listOfTodosId)
        {
            var listOfTodos = await _context.ListsOfTodos.Include(l => l.Todos).FirstOrDefaultAsync(l => l.Id == listOfTodosId);
            return listOfTodos.Todos.Count(x => !x.IsDone);
        }

        public async Task<int> GetNumberOfAllIncompletedTodos(int userId)
        {
            var listsOfTodos = await _context.ListsOfTodos.Include(l => l.Todos).Where(l => l.UserId == userId).ToListAsync();

            return listsOfTodos.Sum(listOfTodo => listOfTodo.Todos.Count(t => !t.IsDone));
        }
    }
}
