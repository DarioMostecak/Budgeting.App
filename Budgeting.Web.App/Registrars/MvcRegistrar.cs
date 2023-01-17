using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Brokers.Storages;
using Budgeting.Web.App.Services.Foundations.Categories;
using Budgeting.Web.App.Services.ProcessingServices.CategoryProcessingServices;

namespace Budgeting.Web.App.Registrars
{
    public class MvcRegistrar : IWebApplicationBuilderRegistrar
    {
        public void RegisterServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();

            #region Add Brokers
            builder.Services.AddSingleton<IStorageBroker, StorageBroker>();//check if is best for optimization
            builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
            #endregion

            #region Add Foundations
            builder.Services.AddTransient<ICategoryService, CategoryService>();
            #endregion

            #region Add Managers
            builder.Services.AddTransient<ICategoryProcessingService, CategoryProcessingService>();
            #endregion
        }
    }
}
