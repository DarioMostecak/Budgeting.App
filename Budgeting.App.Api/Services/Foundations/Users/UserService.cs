using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.UserManagment;
using Budgeting.App.Api.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace Budgeting.App.Api.Services.Foundations.Users
{
    public partial class UserService : IUserService
    {
        private readonly IUserManagerBroker userManagerBroker;
        private readonly ILoggingBroker loggingBroker;

        public UserService(
            IUserManagerBroker userManagerBroker,
            ILoggingBroker loggingBroker)
        {
            this.userManagerBroker = userManagerBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<User> AddUserAsync(User user, string password) =>
        TryCatch(async () =>
        {
            ValidateUserOnCreate(user, password);

            User checkUserEmail =
                await this.userManagerBroker.SelectUserByEmailAsync(user.Email);

            ValidateUserIsNotNull(checkUserEmail);

            IdentityResult identityResult =
                await this.userManagerBroker.InsertUserAsync(user, password);

            ValidateIdentityResultIsFalse(identityResult);

            return await this.userManagerBroker.SelectUserByIdAsync(user.Id);
        });

        public ValueTask<User> RetrieveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserIdIsNull(userId);

            User maybeUser =
                await this.userManagerBroker.SelectUserByIdAsync(userId);

            return maybeUser;
        });
    }
}
