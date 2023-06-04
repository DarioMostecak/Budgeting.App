// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts.Exceptions;
using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;

namespace Budgeting.App.Api.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private delegate ValueTask<User> ReturnigUserFunction();

        private async ValueTask<User> TryCatch(
            ReturnigUserFunction returnigUserFunction)
        {
            try
            {
                return await returnigUserFunction();
            }
            catch (NullUserException nullUserException)
            {
                this.dbTransactionBroker.RollBackTransaction();

                throw CreateAndLogValidationException(nullUserException);
            }
            catch (NullUserPasswordException nullUserPasswordException)
            {
                this.dbTransactionBroker.RollBackTransaction();

                throw CreateAndLogValidationException(nullUserPasswordException);
            }
            catch (FailedOperationUserOrchestrationException failedOperationUserOrchestrationException)
            {
                this.dbTransactionBroker.RollBackTransaction();

                throw CreateAndLogDependencyException(failedOperationUserOrchestrationException);
            }
            catch (UserValidationException userValidationException)
            {
                this.dbTransactionBroker.RollBackTransaction();

                throw CreateAndLogDependencyValidationException(userValidationException);
            }
            catch (UserDependencyException userDependencyException)
            {
                this.dbTransactionBroker.RollBackTransaction();

                throw CreateAndLogDependencyException(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                this.dbTransactionBroker.RollBackTransaction();

                throw CreateAndLogDependencyException(userServiceException);
            }
            catch (AccountValidationException accountValidationException)
            {
                this.dbTransactionBroker.RollBackTransaction();

                throw CreateAndLogDependencyValidationException(accountValidationException);
            }
            catch (AccountDependencyException accountDependencyException)
            {
                this.dbTransactionBroker.RollBackTransaction();

                throw CreateAndLogDependencyException(accountDependencyException);
            }
            catch (AccountServiceException accountServiceException)
            {
                this.dbTransactionBroker.RollBackTransaction();

                throw CreateAndLogDependencyException(accountServiceException);
            }
            finally
            {
                this.dbTransactionBroker.DisposeTransaction();
            }
        }

        private UserOrchestrationDependencyValidationException CreateAndLogDependencyValidationException(Exception exception)
        {
            var userOrchestrationDependencyValidationException =
                new UserOrchestrationDependencyValidationException(exception, exception.Data);

            this.loggingBroker.LogError(userOrchestrationDependencyValidationException);

            return userOrchestrationDependencyValidationException;
        }

        private UserOrchestrationDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var userOrchestrationDependencyException =
                new UserOrchestrationDependencyException(exception);

            this.loggingBroker.LogError(userOrchestrationDependencyException);

            return userOrchestrationDependencyException;
        }

        private UserOrchestrationValidationException CreateAndLogValidationException(Exception exception)
        {
            var userOrchestrationValidationException =
                new UserOrchestrationValidationException(exception);

            this.loggingBroker.LogError(userOrchestrationValidationException);

            return userOrchestrationValidationException;
        }
    }
}
