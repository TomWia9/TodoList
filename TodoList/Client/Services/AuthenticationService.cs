using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TodoList.Client.Shared;
using TodoList.Shared.Auth;
using TodoList.Shared.Dto;

namespace TodoList.Client.Services
{
    public class AuthenticationService :IAuthenticationService
    {
        private readonly NavigationManager _navigationManager;
        private readonly IHttpService _httpService;
        private readonly ILocalStorageService _localStorageService;
        private readonly AppStateContainer _appState;

        public AuthenticateResponse User { get; private set; }

        public AuthenticationService(
            NavigationManager navigationManager,
            ILocalStorageService localStorageService, IHttpService httpService, AppStateContainer appState)
        {
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _httpService = httpService;
            _appState = appState;
        }

        public async Task Initialize()
        {
            User = await _localStorageService.GetItem<AuthenticateResponse>("user");
        }

        public async Task Login(string username, string password)
        {
            var authenticateRequest = new AuthenticateRequest()
            {
                Username = username,
                Password = password
            };
            
            var response = await _httpService.Post("api/users/authenticate", authenticateRequest);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _navigationManager.NavigateTo("logout");
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

                throw new Exception(error["message"]);

            }

            User = await response.Content.ReadFromJsonAsync<AuthenticateResponse>();

            await _localStorageService.SetItem("user", User);

        }

        public async Task Logout()
        {
            User = null;
            await _localStorageService.RemoveItem("user");
            _appState.Clear();
            _navigationManager.NavigateTo("login");
        }

        public async Task<HttpResponseMessage> Register(UserForCreationDto user)
        {
            return await _httpService.Post($"api/users", user);
        }
    }
}
