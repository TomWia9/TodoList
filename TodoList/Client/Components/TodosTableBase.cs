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
        protected AppStateContainer AppState { get; set; }

        protected ListOfTodosDto ListOfTodos { get; set; }
        protected int NumberOfIncompletedTodos { get; set; }
        protected bool ListLoadFailed { get; set; }
        protected string PercentOfDoneTodos { get; set; } = "text-white";
        
        protected DeleteListModal DeleteListModal;
        protected EditListTitleModal EditListTitleModal;
        protected TodoDetailsModal TodoDetailsModal;

        protected string ProgressBarCssClass => PercentOfDoneTodos.Equals("0%") ? "text-dark" : "text-white";

        protected override void OnParametersSet()
        {
            GetListOfTodos();

            if (ListLoadFailed) return;
            GetNumberOfIncompletedTodos();
            GetPercentOfDoneTodos();
        }

        protected void NavigateToNewListComponent()
        {
            NavigationManager.NavigateTo("lists/new");
        }

        private void GetListOfTodos()
        {
            try
            {
                var list = AppState.GetListOfTodos(ListId);

                if (list != null)
                {
                    ListOfTodos = list;
                }
                else
                {
                    ListLoadFailed = true;
                }
            }
            catch
            {
                ListLoadFailed = true;
            }

        }

        private void GetNumberOfIncompletedTodos()
        {
            NumberOfIncompletedTodos = ListOfTodos.Todos.Count(x => !x.IsDone);
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
            GetListOfTodos();
            GetNumberOfIncompletedTodos();
            GetPercentOfDoneTodos();
            await OnUpdated.InvokeAsync();
        }
    }
}
