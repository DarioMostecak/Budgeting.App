using Budgeting.Web.App.Models.UserViews;

namespace Budgeting.Web.App.Services.Views.UserViews
{
    public interface IUserViewService
    {
        ValueTask<UserView> AddUserViewAsync(UserView userView);
        void NavigateTo(string route);
    }
}
