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
            var todoList = await _context.ListsOfTodos.Include(l => l.Todos).FirstOrDefaultAsync(l => l.Id == todoListId);
            todoList.Todos = todoList.Todos.OrderBy(t => t.IsDone).ThenByDescending(t => t.DateAdded);
            return todoList;
        }

        public async Task<IEnumerable<int>> GetTodoListsIdsAsync()
        {
            return await _context.ListsOfTodos.Select(l => l.Id).ToListAsync();
        }

        public async Task<bool> ListOfTodosExists(string title)
        {
            return await _context.ListsOfTodos.AnyAsync(t => t.Title == title);
        }

        public async Task<bool> ListOfTodosExists(int id)
        {
            return await _context.ListsOfTodos.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<ListOfTodos>> GetTodoListsAsync()
        {
            IEnumerable<ListOfTodos> todoLists = await _context.ListsOfTodos.Include(l => l.Todos).ToListAsync();

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

        public async Task<int> GetNumberOfAllIncompletedTodos()
        {
            var listsOfTodos = await _context.ListsOfTodos.Include(l => l.Todos).ToListAsync();

            var numberOfAllIncompletedTodos = 0;

            foreach (var listOfTodo in listsOfTodos)
            {
                numberOfAllIncompletedTodos += listOfTodo.Todos.Count(t => !t.IsDone);
            }

            return numberOfAllIncompletedTodos;
        }
    }
}
