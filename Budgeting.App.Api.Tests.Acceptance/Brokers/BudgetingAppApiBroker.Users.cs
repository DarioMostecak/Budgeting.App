using Budgeting.App.Api.Tests.Acceptance.Models.Users;
using System;
using System.Threading.Tasks;

namespace Budgeting.App.Api.Tests.Acceptance.Brokers
{
    public partial class BudgetingAppApiBroker
    {
        private const string UsersRelativeUrl = "api/v1/Users";

        public async ValueTask<User> PostUserAsync(User user, string password) =>
            await this.PostContentAsync<User>($"{UsersRelativeUrl}?password={password}", user);

        public async ValueTask<User> GetUserByIdAsync(Guid userId) =>
            await this.GetContentAsync<User>($"{UsersRelativeUrl}/{userId}");

        public async ValueTask<User> PutUserAsync(User user) =>
            await this.PutContentAsync<User>(UsersRelativeUrl, user);

        public async ValueTask<User> DeleteUserAsync(Guid userId) =>
            await this.DeleteContentAsync<User>($"{UsersRelativeUrl}/{userId}");
    }
}
