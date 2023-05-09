using Budgeting.Web.App.Brokers.Toasts;
using Budgeting.Web.App.Models.LoginViews;
using Budgeting.Web.App.Models.LoginViews.Exceptions;
using Budgeting.Web.App.Services.Views.LoginViews;
using Budgeting.Web.App.Views.Components.LoginComponents;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor.Services;
using System;
using Tynamix.ObjectFiller;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Views.LoginComponents
{
    public partial class LoginComponentTests : TestContext
    {
        private readonly Mock<ILoginViewService> loginViewServiceMock;
        private readonly Mock<IToastBroker> toastBroker;
        private IRenderedComponent<LoginComponent> renderLoginComponent;

        public LoginComponentTests()
        {
            this.loginViewServiceMock = new Mock<ILoginViewService>();
            this.toastBroker = new Mock<IToastBroker>();
            this.Services.AddTransient(service => this.toastBroker.Object);
            this.Services.AddScoped(services => this.loginViewServiceMock.Object);
            this.Services.AddMudServices();
        }

        private static string GetRandomString() => new MnemonicString().GetValue();

        public static TheoryData LoginViewDependencyServiceException()
        {
            var innerInvalidException = new Exception();

            return new TheoryData<Exception>
            {
                new LoginViewDependencyException(innerInvalidException),
                new LoginViewServiceException(innerInvalidException)
            };
        }

        private static LoginView CreateRandomLoginView() =>
            new LoginView
            {
                Email = "john@gmail.com",
                Password = "12345678"
            };
    }
}
