using Budgeting.Web.App.Models.AuthenticationResults;
using Budgeting.Web.App.Models.LoginViews;

namespace Budgeting.Web.App.Services.Views.LoginViews
{
    public interface ILoginViewService
    {
        ValueTask<AuthenticationResult> LoginAsync(LoginView loginView);
        void NavigateTo(string route);
    }
}
