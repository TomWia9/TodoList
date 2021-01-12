using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using TodoList.Client.Services;
using TodoList.Client.Shared;
using TodoList.Shared.Auth;
using TodoList.Shared.Dto;

namespace TodoList.Client.Pages
{
    public class NewListOfTodosBase : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        [Inject]
        protected ITodoListsService TodoListsService { get; set; }
        [Inject]
        protected AppStateContainer AppState { get; set; }
        
        [Inject] 
        protected ILocalStorageService LocalStorageService { get; set; }

        protected ListOfTodosForCreationDto ListOfTodos { get; set; } = new();
        protected bool CreationFailed { get; set; }
        protected bool ListAlreadyExists { get; set; }

        protected async Task CreateList()
        {
            var user = await LocalStorageService.GetItem<AuthenticateResponse>("user");

            ListOfTodos.UserId = user.Id;

            var response = await TodoListsService.CreateList(ListOfTodos);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                ListAlreadyExists = true;
            }
            else if (!response.IsSuccessStatusCode)
            {
                CreationFailed = true;
            }
            else
            {
                var listOfTodos = await response.Content.ReadFromJsonAsync<ListOfTodosDto>();
                if (listOfTodos != null)
                {
                    AppState.AddListOfTodos(listOfTodos);
                    var url = $"lists/{listOfTodos.Id}";
                    NavigationManager.NavigateTo(url);
                }
                else
                {
                    CreationFailed = true;
                }
            }
        }
    }
}
