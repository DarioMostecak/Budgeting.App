using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;

namespace Budgeting.App.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private static void ValidateApplicationUserOnCreate(User user)
        {
            ValidateUserIsNull(user);
        }

        private static void ValidateUserIsNull(User user)
        {
            if (user is null) throw new NullUserException();
        }
    }
}
