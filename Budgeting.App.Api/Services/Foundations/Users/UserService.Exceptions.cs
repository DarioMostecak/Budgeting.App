using Budgeting.App.Api.Models.Users;

namespace Budgeting.App.Api.Services.Foundations.Users
{
    public partial class UserService
    {
        private delegate ValueTask<User> ReturningUserFunctions();
        private delegate IQueryable<User> ReturningUsersFunctions();

        private async ValueTask<User> TryCatch(
            ReturningUserFunctions returningUserFunctions)
        {
            try
            {
                return await returningUserFunctions();
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private IQueryable<User> TryCatch(
            ReturningUsersFunctions returningUsersFunctions)
        {
            try
            {
                return returningUsersFunctions();
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}
