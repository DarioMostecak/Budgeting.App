using Budgeting.Web.App.Models.Users;

namespace Budgeting.Web.App.Services.Foundations.Users
{
    public interface IUserService
    {
        ValueTask<User> AddUserAsync(User user, string password);
    }
}
