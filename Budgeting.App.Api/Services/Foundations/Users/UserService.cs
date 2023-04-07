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

            ValidateStorageUser(
                inputUserId: userId,
                storageUser: maybeUser);

            return maybeUser;
        });

        public ValueTask<User> ModifyUserAsync(User user) =>
        TryCatch(async () =>
        {
            ValidateUserOnModify(user);

            User maybeUser =
                await this.userManagerBroker.SelectUserByIdAsync(user.Id);

            ValidateStorageUser(
                inputUserId: user.Id,
                storageUser: maybeUser);

            ValidateAgainstStorageUserOnModify(
                inputUser: user,
                storageUser: maybeUser);

            IdentityResult identityResult =
                await this.userManagerBroker.UpdateUserAsync(user);

            ValidateIdentityResultIsFalse(identityResult);

            return user;
        });

        public ValueTask<User> RemoveUserByIdAsync(Guid userId) =>
        TryCatch(async () =>
        {
            ValidateUserIdIsNull(userId);

            User maybeUser = await this.userManagerBroker.SelectUserByIdAsync(userId);

            ValidateStorageUser(
                inputUserId: userId,
                storageUser: maybeUser);

            IdentityResult identityResult =
                 await this.userManagerBroker.DeleteUserAsync(maybeUser);

            ValidateIdentityResultIsFalse(identityResult);

            return maybeUser;

        });
    }
}
