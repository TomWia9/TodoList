﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TodoList.Shared.Dto;

namespace TodoList.Client.Pages
{
    public class AllTodoBase : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }
        public IEnumerable<ListOfTodosDto> ListsOfTodos { get; set; } = new List<ListOfTodosDto>();
        public int NumberOfIncompletedTodos { get; set; }

        //have to change it to get just ids instead of all full lists
        protected override async Task OnInitializedAsync()
        {
            await GetListsOfTodosAsync();

            await GetNumberOfIncompetedTodos();
        }

        protected async Task GetNumberOfIncompetedTodos()
        {
            await GetListsOfTodosAsync();

            if (ListsOfTodos == null)
                return;

            NumberOfIncompletedTodos = 0;

            foreach (var list in ListsOfTodos)
            {
                NumberOfIncompletedTodos += list.Todos.Count(t => !t.IsDone);
            }

        }

        private async Task GetListsOfTodosAsync()
        {
            ListsOfTodos = await HttpClient.GetFromJsonAsync<IEnumerable<ListOfTodosDto>>("api/lists");
        }
    }
}
