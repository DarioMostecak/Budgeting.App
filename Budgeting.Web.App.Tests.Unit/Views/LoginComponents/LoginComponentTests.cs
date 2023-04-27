using Budgeting.Web.App.Services.Views.LoginViews;
using Budgeting.Web.App.Views.Components.LoginComponents;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using MudBlazor;
using MudBlazor.Services;

namespace Budgeting.Web.App.Tests.Unit.Views.LoginComponents
{
    public partial class LoginComponentTests : TestContext
    {
        //add ISnacbar
        private readonly Mock<ILoginViewService> loginViewServiceMock;
        private readonly Mock<ISnackbar> snackbarService;
        private IRenderedComponent<LoginComponent> loginComponentMock;

        public LoginComponentTests()
        {
            this.loginViewServiceMock = new Mock<ILoginViewService>();
            this.snackbarService = new Mock<ISnackbar>();
            this.Services.AddScoped(services => this.loginViewServiceMock.Object);
            this.Services.AddMudServices();
        }
    }
}
