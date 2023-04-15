using Budgeting.Web.App.Models.AuthenticationResults;

namespace Budgeting.Web.App.Services.Foundations
{
    public partial class IdentityService
    {
        private delegate ValueTask<AuthenticationResult> AuthenticateReturnigFunction();

        private async ValueTask<AuthenticationResult> TryCatch(
            AuthenticateReturnigFunction authenticateReturnigFunction)
        {
            try
            {
                return await authenticateReturnigFunction();
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
