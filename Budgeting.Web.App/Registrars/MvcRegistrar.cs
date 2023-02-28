using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.DateTimes;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Services.Foundations.Categories;
using Budgeting.Web.App.Services.Views.CategoryViews;

namespace Budgeting.Web.App.Registrars
{
    public class MvcRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient<IApiBroker, ApiBroker>();


            #region Add Brokers
            builder.Services.AddScoped<IApiBroker, ApiBroker>();
            builder.Services.AddScoped<ILogger, Logger<LoggingBroker>>();
            builder.Services.AddScoped<ILoggingBroker, LoggingBroker>();
            builder.Services.AddScoped<IDateTimeBroker, DateTimeBroker>();
            #endregion

            #region Add Foundations
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            #endregion

            #region Add ProccesService
            builder.Services.AddScoped<ICategoryViewService, CategoryViewService>();
            #endregion
        }
    }
}
