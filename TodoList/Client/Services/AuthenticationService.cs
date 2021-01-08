using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TodoList.Shared.Auth;

namespace TodoList.Client.Services
{
    public class AuthenticationService :IAuthenticationService
    {
        private readonly NavigationManager _navigationManager;
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorageService;

        public AuthenticateResponse User { get; private set; }

        public AuthenticationService(
            NavigationManager navigationManager,
            ILocalStorageService localStorageService, HttpClient http)
        {
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _http = http;
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
            
            var response = await _http.PostAsJsonAsync("api/users/authenticate", authenticateRequest);

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
            _navigationManager.NavigateTo("login");
        }
    }
}
