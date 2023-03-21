using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;

namespace Budgeting.App.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private static void ValidateUserOnCreate(User user, string password)
        {
            ValidateUserIsNull(user);
            ValidatePasswordIsNull(password);
        }

        private static void ValidateUserIsNull(User user)
        {
            if (user is null) throw new NullUserException();
        }

        private static void ValidatePasswordIsNull(string password)
        {
            if (password is null) throw new NullUserPasswordException();
        }
    }
}
