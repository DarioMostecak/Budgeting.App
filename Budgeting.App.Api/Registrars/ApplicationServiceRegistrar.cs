using Budgeting.App.Api.Brokers.DateTimes;
using Budgeting.App.Api.Brokers.DbTransactions;
using Budgeting.App.Api.Brokers.Loggings;
using Budgeting.App.Api.Brokers.Storages;
using Budgeting.App.Api.Brokers.UniqueIDGenerators;
using Budgeting.App.Api.Brokers.UserManagers;
using Budgeting.App.Api.Services.Foundations.Accounts;
using Budgeting.App.Api.Services.Foundations.Categories;
using Budgeting.App.Api.Services.Foundations.Users;
using Budgeting.App.Api.Services.Identity;

namespace Budgeting.App.Api.Registrars
{
    public class ApplicationServiceRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            #region Add Brokers
            builder.Services.AddSingleton<IStorageBroker, StorageBroker>();
            builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
            builder.Services.AddTransient<IDateTimeBroker, DateTimeBroker>();
            builder.Services.AddTransient<IUserManagerBroker, UserManagerBroker>();
            builder.Services.AddTransient<IDbTransactionBroker, DbTransactionBroker>();
            builder.Services.AddTransient<IUniqueIDGeneratorBroker, UniqueIDGeneratorBroker>();
            #endregion

            #region Add Foundations
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<IAccountService, AccountService>();
            #endregion

            #region Add Identity
            builder.Services.AddTransient<IIdentityService, IdentityService>();
            #endregion

            #region Add Orchestration
            #endregion
        }
    }
}
