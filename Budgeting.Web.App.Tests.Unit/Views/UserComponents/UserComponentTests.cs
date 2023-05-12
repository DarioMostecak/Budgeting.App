using Budgeting.Web.App.Brokers.Toasts;
using Budgeting.Web.App.Models.UserViews;
using Budgeting.Web.App.Models.UserViews.Exceptions;
using Budgeting.Web.App.Services.Views.UserViews;
using Budgeting.Web.App.Views.Components.UserComponents;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor.Services;
using System;
using Tynamix.ObjectFiller;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Views.UserComponents
{
    public partial class UserComponentTests : TestContext
    {
        private readonly Mock<IUserViewService> userViewServiceMock;
        private readonly Mock<IToastBroker> toastBroker;
        private IRenderedComponent<UserRegisterComponent> renderUserRegisterComponent;

        public UserComponentTests()
        {
            this.userViewServiceMock = new Mock<IUserViewService>();
            this.toastBroker = new Mock<IToastBroker>();
            this.Services.AddTransient(service => this.toastBroker.Object);
            this.Services.AddScoped(services => this.userViewServiceMock.Object);
            this.Services.AddMudServices();
        }

        private static string GetRandomString() => new MnemonicString().GetValue();

        public TheoryData UserViewValidationExceptions()
        {
            string randomMessage = GetRandomString();
            string validationMesage = randomMessage;
            var innerValidationException = new Exception(validationMesage);

            return new TheoryData<Exception>
            {
                new UserViewValidationException(innerValidationException, innerValidationException.Data),
                new UserViewDependencyException(innerValidationException)
            };
        }

        public TheoryData UserViewDependencyServiceExceptions()
        {
            var innerInvalidException = new Exception();

            return new TheoryData<Exception>
            {
                new UserViewDependencyException(innerInvalidException),
                new UserViewServiceException(innerInvalidException)
            };
        }

        public static UserView CreateRandomUserView() =>
            new UserView
            {
                FirstName = "Test",
                LastName = "Test1",
                Email = "Test@mail.com",
                Password = "11111111",
                ConfirmPassword = "11111111"
            };
    }
}
