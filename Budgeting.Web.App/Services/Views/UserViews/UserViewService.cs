using Budgeting.Web.App.Brokers.DateTimes;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Brokers.Navigations;
using Budgeting.Web.App.Brokers.UniqueIDGenerators;
using Budgeting.Web.App.Models.Users;
using Budgeting.Web.App.Models.UserViews;
using Budgeting.Web.App.Services.Foundations.Users;

namespace Budgeting.Web.App.Services.Views.UserViews
{
    public partial class UserViewService : IUserViewService
    {
        private readonly IUserService userService;
        private readonly ILoggingBroker loggingBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly IUniqueIDGeneratorBroker uniqueIDGeneratorBroker;
        private readonly INavigationBroker navigationBroker;

        public UserViewService(
            IUserService userService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IUniqueIDGeneratorBroker uniqueIDGeneratorBroker,
            INavigationBroker navigationBroker)
        {
            this.userService = userService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.uniqueIDGeneratorBroker = uniqueIDGeneratorBroker;
            this.navigationBroker = navigationBroker;
        }

        public ValueTask<UserView> AddUserViewAsync(UserView userView) =>
        TryCatch(async () =>
        {
            ValidateUserViewOnAdd(userView);

            User user = MapToUserOnAdd(userView);

            await this.userService.AddUserAsync(
                user: user,
                password: userView.Password);

            return userView;
        });

        private User MapToUserOnAdd(UserView userView)
        {
            Guid id =
                this.uniqueIDGeneratorBroker.GenerateUniqueID();

            DateTime currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            return new User
            {
                Id = id,
                FirstName = userView.FirstName,
                LastName = userView.LastName,
                Email = userView.Email,
                CreatedDate = currentDateTime,
                UpdatedDate = currentDateTime
            };
        }


        private User MapToUserOnModify(UserView userView)
        {
            DateTime currentDateTime =
                this.dateTimeBroker.GetCurrentDateTime();

            return new User
            {
                Id = userView.Id,
                FirstName = userView.FirstName,
                LastName = userView.LastName,
                Email = userView.Email,
                CreatedDate = userView.CreatedDate,
                UpdatedDate = currentDateTime
            };
        }




    }
}
