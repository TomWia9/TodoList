using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TodoList.Shared.Dto;

namespace TodoList.Client.Pages
{
    public class NewTodoBase : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public int ListId { get; set; }

        protected TodoForCreationDto TodoForCreation { get; set; } = new TodoForCreationDto();
        protected  bool TodoAlreadyExists { get; set; }
        protected bool? CreationFailed { get; set; }


        protected async Task CreateTodo()
        {
            Console.WriteLine("CreateTodo");
            Console.WriteLine("ListId: {0}", ListId);
            Console.WriteLine("Title: {0}", TodoForCreation.Title);

            var response = await HttpClient.PostAsJsonAsync($"api/lists/{ListId}/todos", TodoForCreation);

            if (response.StatusCode == HttpStatusCode.Conflict)
            {
                TodoAlreadyExists = true;
            }
            else if (!response.IsSuccessStatusCode)
            {
                CreationFailed = true;
            }
            else
            {
                //reload page to display new todo
                //maybe form reset or something
                CreationFailed = false;
                RefreshPage();
            }
        }

        protected void RefreshPage()
        {
            NavigationManager.NavigateTo($"refreshList/{ListId}");
        }
    }
}
