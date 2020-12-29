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

        public event Action OnListsUpdate;

        public void AddListsOfTodos(IEnumerable<ListOfTodosDto> listsOfTodos)
        {
            ListsOfTodos = listsOfTodos;
            NotifyStateChanged();
        }

        public void AddListOfTodos(ListOfTodosDto listsOfTodos)
        {
            var lists = ListsOfTodos.ToList();
            lists.Add(listsOfTodos);
            ListsOfTodos = lists;
            NotifyStateChanged();
        }

        public void RemoveListOfTodos(int listId)
        {

            var lists = ListsOfTodos.ToList();
            var listToRemove = lists.Single(l => l.Id == listId);

            lists.Remove(listToRemove);
            ListsOfTodos = lists;
            NotifyStateChanged();
        }
        
        private void NotifyStateChanged() => OnListsUpdate?.Invoke();

    }
}
