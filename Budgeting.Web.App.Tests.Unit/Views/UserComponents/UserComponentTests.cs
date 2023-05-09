using Budgeting.Web.App.Brokers.Toasts;
using Budgeting.Web.App.Services.Views.UserViews;
using Budgeting.Web.App.Views.Components.UserComponents;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor.Services;

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
    }
}
