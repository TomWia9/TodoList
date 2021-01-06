using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TodoList.Client.Components.Modals;
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

        protected ListOfTodosDto ListOfTodos { get; set; }
        protected int NumberOfIncompletedTodos { get; set; } = 0;
        protected bool ListLoadFailed { get; set; }
        protected string PercentOfDoneTodos { get; set; }
        
        protected DeleteListModal DeleteListModal;
        protected EditListTitleModal EditListTitleModal;
        protected TodoDetailsModal TodoDetailsModal;

        protected string ProgressBarCssClass => PercentOfDoneTodos.Equals("0%") ? "text-dark" : null;

        protected override async Task OnParametersSetAsync()
        {
            await GetListOfTodos();

            if (!ListLoadFailed)
            {
                await GetNumberOfIncompletedTodos();
                GetPercentOfDoneTodos();
            }
        }

        protected void NavigateToNewListComponent()
        {
            NavigationManager.NavigateTo("lists/new");
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
