using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TodoList.Client.Components.Modals;
using TodoList.Client.Services;
using TodoList.Client.Shared;
using TodoList.Shared.Dto;

namespace TodoList.Client.Components
{
    public class TodoElementBase : ComponentBase
    {
        [Parameter]
        public TodoDto Todo { get; set; }
        
        [Parameter]
        public EventCallback OnUpdated { get; set; }

        [Parameter]
        public EventCallback OnDetailsClick { get; set; }

        [Inject]
        protected ITodosService TodosService { get; set; }

        [Inject]
        protected AppStateContainer AppState { get; set; }

        protected bool UpdateFailed { get; set; }
        protected bool DeleteFailed { get; set; }

        protected async Task UpdateStatus()
        {
            try
            {
                var response = await TodosService.UpdateStatus(Todo);
                if (response.IsSuccessStatusCode)
                {
                    AppState.UpdateTodo(Todo);
                    await OnUpdated.InvokeAsync();
                }
                else
                {
                    UpdateFailed = true;

                }
            }
            catch
            {
                UpdateFailed = true;
            }
        }

        protected async Task DeleteTodo()
        {
            try
            {
                var response = await TodosService.DeleteTodo(Todo.ListOfTodosId, Todo.Id);
                if (response.IsSuccessStatusCode)
                {
                    AppState.DeleteTodo(Todo);
                    await OnUpdated.InvokeAsync();
                }
                else
                {
                    DeleteFailed = true;

                }
                await OnUpdated.InvokeAsync();
            }
            catch
            {
                DeleteFailed = true;
            }
        }
    }
}
