using Budgeting.Web.App.Models.Users;

namespace Budgeting.Web.App.Brokers.Apis
{
    public partial class ApiBroker
    {
        private const string UserRelativeUrl = "";

        public async ValueTask<User> PostUserAsync(User user, string password) =>
            await this.PostContentAsync(UserRelativeUrl + $"/{password}", user);
    }
}
