// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace Budgeting.App.Api.Brokers.UserManagment
{
    public class UserManagerBroker : IUserManagerBroker
    {
        private readonly UserManager<User> userManager;

        public UserManagerBroker(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async ValueTask<User> SelectUserByEmailAsync(string email) =>
            await this.userManager.FindByEmailAsync(email);

        public async ValueTask<IdentityResult> InsertUserAsync(User user, string password) =>
            await this.userManager.CreateAsync(user, password);

        public async ValueTask<User> SelectUserByIdAsync(Guid userId) =>
            await this.userManager.FindByIdAsync(userId.ToString());

        public async ValueTask<IdentityResult> UpdateUserAsync(User user) =>
            await this.userManager.UpdateAsync(user);

        public async ValueTask<IdentityResult> DeleteUserAsync(User user) =>
            await this.userManager.DeleteAsync(user);

        public async ValueTask<bool> ConfirmUserByPasswordAsync(User user, string password) =>
            await this.userManager.CheckPasswordAsync(user, password);
    }
}
