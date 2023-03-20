using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.UserManagment;
using Budgeting.App.Api.Models.Users;

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
                return user;
            });
    }
}
