using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TodoList.Client.Components.Modals;
using TodoList.Client.Services;
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
        
        protected bool UpdateFailed { get; set; }
        protected bool DeleteFailed { get; set; }

        protected async Task UpdateStatus()
        {
            try
            {
                var response = await TodosService.UpdateStatus(Todo);
                UpdateFailed = !response.IsSuccessStatusCode;
                await OnUpdated.InvokeAsync();
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
                DeleteFailed = !response.IsSuccessStatusCode;
                await OnUpdated.InvokeAsync();
            }
            catch
            {
                DeleteFailed = true;
            }
        }
    }
}
