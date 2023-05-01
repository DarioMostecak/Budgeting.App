using Budgeting.Web.App.Brokers.DateTimes;
using Budgeting.Web.App.Brokers.Loggings;
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

        public UserViewService(
            IUserService userService,
            ILoggingBroker loggingBroker,
            IDateTimeBroker dateTimeBroker,
            IUniqueIDGeneratorBroker uniqueIDGeneratorBroker)
        {
            this.userService = userService;
            this.loggingBroker = loggingBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.uniqueIDGeneratorBroker = uniqueIDGeneratorBroker;
        }

        public ValueTask<UserView> AddUserAsync(UserView userView) =>
        TryCatch(async () =>
        {
            //validate user view

            User user = MapToUserOnAdd(userView);

            await this.userService.AddUserAsync(
                user: user,
                password: userView.Password);

            return userView;
        });

        private User MapToUserOnAdd(UserView userView) =>
            new User
            {
                Id = this.uniqueIDGeneratorBroker.GenerateUniqueID(),
                FirstName = userView.FirstName,
                LastName = userView.LastName,
                Email = userView.Email,
                CreatedDate = this.dateTimeBroker.GetCurrentDateTime(),
                UpdatedDate = this.dateTimeBroker.GetCurrentDateTime()
            };

        private User MapToUserOnModify(UserView userView) =>
            new User
            {
                Id = userView.Id,
                FirstName = userView.FirstName,
                LastName = userView.LastName,
                Email = userView.Email,
                CreatedDate = userView.CreatedDate,
                UpdatedDate = this.dateTimeBroker.GetCurrentDateTime()
            };



    }
}
