using Budgeting.App.Api.Models.Users;

namespace Budgeting.App.Api.Services.Foundations.Users
{
    public interface IUserService
    {
        ValueTask<User> AddUserAsync(User user, string password);
        ValueTask<User> RetrieveUserById(Guid userId);
    }
}
