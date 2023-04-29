using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.DateTimes;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Models.Users;

namespace Budgeting.Web.App.Services.Foundations.Users
{
    public partial class UserService : IUserService
    {
        private readonly IApiBroker apiBroker;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;

        public UserService(
            IApiBroker apiBroker,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker)
        {
            this.apiBroker = apiBroker;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
        }

        public ValueTask<User> AddUserAsync(User user, string passwor) =>
        TryCatch(async () =>
        {
            //Validate user and password (ValidateUserIsNull , ValidateUserOnCreate)

            User newUser =
                 await this.apiBroker.PostUserAsync(user, passwor);

            //validate user created

            return newUser;
        });



    }
}
