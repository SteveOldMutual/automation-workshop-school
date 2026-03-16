using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;

namespace Automation_School_UI.Components
{
    public class AuthenticatedComponentBase : LayoutComponentBase
    {
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }
        [Inject] protected NavigationManager Navigation { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var token = await LocalStorageService.GetItemAsync<string>("authToken");
            if (string.IsNullOrEmpty(token))
            {
                Navigation.NavigateTo("/login");
            }
        }
    }
}
