using Budgeting.Web.App.Models.Users.Exceptions;
using Budgeting.Web.App.Models.UserViews;
using Budgeting.Web.App.Models.UserViews.Exceptions;

namespace Budgeting.Web.App.Services.Views.UserViews
{
    public partial class UserViewService
    {
        private delegate ValueTask<UserView> UserReturningFunctions();

        private async ValueTask<UserView> TryCatch(
            UserReturningFunctions userReturningFunctions)
        {
            try
            {
                return await userReturningFunctions();
            }
            catch (NullUserViewException nullUserViewException)
            {
                throw CreateAndLogValidationException(nullUserViewException);
            }
            catch (InvalidUserViewException invalidUserViewException)
            {
                throw CreateAndLogValidationException(invalidUserViewException);
            }
            catch (UserDependencyValidationException userDependencyValidationException)
            {
                throw CreateAndLogDependencyValidationException(userDependencyValidationException);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        private UserViewValidationException CreateAndLogValidationException(Exception exception)
        {
            var userViewValidationException =
                new UserViewValidationException(exception, exception.Data);

            this.loggingBroker.LogError(userViewValidationException);

            return userViewValidationException;
        }

        private UserViewDependencyValidationException CreateAndLogDependencyValidationException(Exception exception)
        {
            var userViewDependencyValidationException =
               new UserViewDependencyValidationException(exception);

            this.loggingBroker.LogError(userViewDependencyValidationException);

            return userViewDependencyValidationException;
        }


    }
}
