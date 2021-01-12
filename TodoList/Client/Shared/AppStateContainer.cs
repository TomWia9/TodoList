using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Shared.Dto;

namespace TodoList.Client.Shared
{
    public class AppStateContainer
    {
        public IEnumerable<ListOfTodosDto> ListsOfTodos = new List<ListOfTodosDto>();
        public int? NumberOfAllIncompletedTodos;

        public event Action OnListsUpdate;

        public void AddListsOfTodos(IEnumerable<ListOfTodosDto> listsOfTodos)
        {
            ListsOfTodos = listsOfTodos;
            GetNumberOfAllIncompletedTodos();
            NotifyStateChanged();
        }

        public void AddListOfTodos(ListOfTodosDto listsOfTodos)
        {
            var lists = ListsOfTodos.ToList();
            lists.Add(listsOfTodos);
            ListsOfTodos = lists;
            GetNumberOfAllIncompletedTodos();
            NotifyStateChanged();
        }

        public void RemoveListOfTodos(int listId)
        {

            var lists = ListsOfTodos.ToList();
            var listToRemove = lists.Single(l => l.Id == listId);

            lists.Remove(listToRemove);
            ListsOfTodos = lists;
            GetNumberOfAllIncompletedTodos();
            NotifyStateChanged();
        }

        public void UpdateListTitle(int listId, string newTitle)
        {

            var lists = ListsOfTodos.ToList();
            var index = lists.FindIndex(l => l.Id == listId);
            
            if (index > -1)
                lists[index].Title = newTitle;
            
            ListsOfTodos = lists;
            NotifyStateChanged();
        }

        public void AddTodo(TodoDto todo)
        {
            var lists = ListsOfTodos.ToList();
            var listIndex = lists.FindIndex(l => l.Id == todo.ListOfTodosId);

            if (listIndex > -1)
            {
                var todos = lists[listIndex].Todos.ToList();
                todos.Add(todo);
                lists[listIndex].Todos = todos;
            }

            ListsOfTodos = lists;
            GetNumberOfAllIncompletedTodos();
            NotifyStateChanged();
        }
        public void UpdateTodo(TodoDto todo)
        {
            var lists = ListsOfTodos.ToList();
            var listIndex = lists.FindIndex(l => l.Id == todo.ListOfTodosId);

            if (listIndex > -1)
            {
                var todos = lists[listIndex].Todos.ToList();
                var todoIndex = todos.FindIndex(t => t.Id == todo.Id);

                if (todoIndex > -1)
                    todos[todoIndex] = todo;

                lists[listIndex].Todos = todos;

            }

            ListsOfTodos = lists;
            GetNumberOfAllIncompletedTodos();
            NotifyStateChanged();
        }

        public void DeleteTodo(TodoDto todo)
        {
            var lists = ListsOfTodos.ToList();
            var listIndex = lists.FindIndex(l => l.Id == todo.ListOfTodosId);

            if (listIndex > -1)
            {
                var todos = lists[listIndex].Todos.ToList();
                todos.RemoveAll(t => t.Id == todo.Id);
                lists[listIndex].Todos = todos;

            }

            ListsOfTodos = lists;
            GetNumberOfAllIncompletedTodos();
            NotifyStateChanged();
        }

        public void Clear()
        {
            ListsOfTodos = new List<ListOfTodosDto>();
            NumberOfAllIncompletedTodos = null;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnListsUpdate?.Invoke();

        private void GetNumberOfAllIncompletedTodos()
        {
            NumberOfAllIncompletedTodos = ListsOfTodos.Sum(listOfTodo => listOfTodo.Todos.Count(t => !t.IsDone));
        }

    }
}
