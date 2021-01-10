using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using TodoList.Client.Helpers.ExtensionMethods;
using TodoList.Client.Services;
using TodoList.Shared.Auth;

namespace TodoList.Client.Components
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        
        [Inject]
        protected IAuthenticationService AuthenticationService { get; set; }
        
        protected readonly AuthenticateRequest AuthenticateRequest = new();
        protected bool Loading;
        protected string Error;
        protected async void HandleValidSubmit()
        {
            Loading = true;
            try
            {
                await AuthenticationService.Login(AuthenticateRequest.Username, AuthenticateRequest.Password);
                var returnUrl = NavigationManager.QueryString("returnUrl") ?? "/";
                NavigationManager.NavigateTo(returnUrl);
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                Loading = false;
                StateHasChanged();
            }
        }
    }
}
