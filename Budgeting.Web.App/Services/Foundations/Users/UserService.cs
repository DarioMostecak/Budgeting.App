using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Models.Users;

namespace Budgeting.Web.App.Services.Foundations.Users
{
    public partial class UserService : IUserService
    {
        private readonly IApiBroker apiBroker;
        private readonly ILoggingBroker loggingBroker;

        public UserService(
            IApiBroker apiBroker,
            ILoggingBroker loggingBroker)
        {
            this.apiBroker = apiBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<User> AddUserAsync(User user, string password) =>
        TryCatch(async () =>
        {

            ValidateUserAndPasswordOnCreate(user, password);

            return await this.apiBroker.PostUserAsync(user, password);
        });



    }
}
