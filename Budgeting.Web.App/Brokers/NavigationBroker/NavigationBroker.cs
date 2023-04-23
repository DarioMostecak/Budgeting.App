using Microsoft.AspNetCore.Components;

namespace Budgeting.Web.App.Brokers.NavigationBroker
{
    public class NavigationBroker : INavigationBroker
    {
        private readonly NavigationManager navigationManager;

        public NavigationBroker(NavigationManager navigationManager) =>
            this.navigationManager = navigationManager;

        public void NavigateTo(string route) =>
            this.navigationManager.NavigateTo(route);
    }
}
