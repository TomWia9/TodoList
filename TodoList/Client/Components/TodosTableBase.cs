using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TodoList.Client.Services;
using TodoList.Client.Shared;
using TodoList.Shared.Dto;

namespace TodoList.Client.Components
{
    public class TodosTableBase : ComponentBase
    {
        [Parameter]
        public int ListId { get; set; }

        [Parameter]
        public EventCallback OnUpdated { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected TodoListsService TodoListsService { get; set; }

        [Inject]
        protected TodosService TodosService { get; set; }

        protected ListOfTodosDto ListOfTodos { get; set; }
        protected string ListTitle { get; set; }
        protected int NumberOfIncompletedTodos { get; set; } = 0;
        protected bool ListLoadFailed { get; set; }
        protected bool UpdateFailed { get; set; }
        protected bool DeleteFailed { get; set; }
        protected string PercentOfDoneTodos { get; set; }
        
        protected DeleteListModal DeleteListModal;
        protected TodoDetailsModal TodoDetailsModal;
        protected override async Task OnParametersSetAsync()
        {
            await GetListOfTodos();

            if (!ListLoadFailed)
            {
                ListTitle = ListOfTodos.Title;
                await GetNumberOfIncompletedTodos();
                GetPercentOfDoneTodos();
            }
        }

        protected void NavigateToNewListComponent()
        {
            NavigationManager.NavigateTo("lists/new");
        }

        protected async Task UpdateStatus(TodoDto todo)
        {
            try
            {
                var response = await TodosService.UpdateStatus(todo);
                UpdateFailed = !response.IsSuccessStatusCode;
                await ReloadListOfTodos();
            }
            catch
            {
                UpdateFailed = true;
            }
        }

        protected async Task DeleteTodo(int todoId)
        {
            try
            {
                var response = await TodosService.DeleteTodo(ListId, todoId);
                DeleteFailed = !response.IsSuccessStatusCode;
                await ReloadListOfTodos();
            }
            catch
            {
                DeleteFailed = true;
            }
        }

        private async Task GetListOfTodos()
        {
            try
            {
                ListOfTodos = await TodoListsService.GetListOfTodosAsync(ListId);

                ListLoadFailed = ListOfTodos == null;
            }
            catch
            {
                ListLoadFailed = true;
            }

        }

        private async Task GetNumberOfIncompletedTodos()
        {
            NumberOfIncompletedTodos = await TodoListsService.GetNumberOfIncompletedTodosAsync(ListId);
        }

        private void GetPercentOfDoneTodos()
        {
            if (NumberOfIncompletedTodos == 0)
            {
                PercentOfDoneTodos = "100%";
            }
            else
            {
                var percent = (int)((ListOfTodos.Todos.Count() - NumberOfIncompletedTodos) / (ListOfTodos.Todos.Count() / 100.00));

                PercentOfDoneTodos = percent + "%";
            }
        }

        protected async Task ReloadListOfTodos()
        {
            await GetListOfTodos();
            await GetNumberOfIncompletedTodos();
            GetPercentOfDoneTodos();
            await OnUpdated.InvokeAsync();
        }
    }
}
