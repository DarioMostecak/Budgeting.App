using Blazored.LocalStorage;
using Budgeting.Web.App;
using Budgeting.Web.App.Brokers.Apis;
using Budgeting.Web.App.Brokers.AuthenticationProviders;
using Budgeting.Web.App.Brokers.DateTimes;
using Budgeting.Web.App.Brokers.Loggings;
using Budgeting.Web.App.Brokers.Navigations;
using Budgeting.Web.App.Brokers.Toasts;
using Budgeting.Web.App.Brokers.UniqueIDGenerators;
using Budgeting.Web.App.Services.Foundations;
using Budgeting.Web.App.Services.Foundations.Categories;
using Budgeting.Web.App.Services.Foundations.Identity;
using Budgeting.Web.App.Services.Foundations.Users;
using Budgeting.Web.App.Services.Views.LoginViews;
using Budgeting.Web.App.Services.Views.UserViews;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


#region MudBlazor configuration
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;

    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 10000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});
#endregion

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationProviderBroker>();
builder.Services.AddScoped<AuthenticationStateProvider>(service => service.GetRequiredService<AuthenticationProviderBroker>());

#region Brokers
builder.Services.AddTransient<IApiBroker, ApiBroker>();
builder.Services.AddTransient<ILoggingBroker, LoggingBroker>();
builder.Services.AddTransient<IDateTimeBroker, DateTimeBroker>();
builder.Services.AddTransient<INavigationBroker, NavigationBroker>();
builder.Services.AddScoped<IAuthenticationProviderBroker, AuthenticationProviderBroker>();
builder.Services.AddTransient<IToastBroker, ToastBroker>();
builder.Services.AddTransient<IUniqueIDGeneratorBroker, UniqueIDGeneratorBroker>();
#endregion

#region Foudations
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
#endregion

#region Views
builder.Services.AddTransient<ILoginViewService, LoginViewService>();
builder.Services.AddTransient<IUserViewService, UserViewService>();
#endregion

builder.Services.AddHttpClient<IApiBroker, ApiBroker>();


await builder.Build().RunAsync();
