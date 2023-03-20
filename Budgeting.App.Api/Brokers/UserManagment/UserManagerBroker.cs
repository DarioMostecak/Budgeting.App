using Budgeting.App.Api.Models.Users;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

        public async ValueTask<IdentityResult> InsertClaimsAsync(User user, IEnumerable<Claim> claims) =>
            await this.userManager.AddClaimsAsync(user, claims);
    }
}
