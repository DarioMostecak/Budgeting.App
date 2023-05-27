// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
using Budgeting.App.Api.Models.Accounts.Exceptions;
using MongoDB.Driver;

namespace Budgeting.App.Api.Services.Foundations.Accounts
{
    public partial class AccountService
    {
        private delegate ValueTask<Account> AccountReturnigFunctions();

        private async ValueTask<Account> TryCatch(
            AccountReturnigFunctions accountReturnigFunctions)
        {
            try
            {
                return await accountReturnigFunctions();
            }
            catch (NullAccountException nullAccountException)
            {
                throw CreateAndLogValidationException(nullAccountException);
            }
            catch (InvalidAccountException invalidAccountException)
            {
                throw CreateAndLogValidationException(invalidAccountException);
            }
            catch (NotFoundAccountException notFoundAccountException)
            {
                throw CreateAndLogValidationException(notFoundAccountException);
            }
            catch (MongoWriteException mongoWriteException)
              when (GetErrorCode(mongoWriteException) == 11000)
            {
                var alreadyExistsAccountException =
                    new AlreadyExistsAccountExceptions(mongoWriteException);

                throw CreateAndLogValidationException(alreadyExistsAccountException);
            }
            catch (MongoException mongoException)
            {
                var failedAccountDependencyException =
                    new FailedAccountDependencyException(mongoException);

                throw CreateAndLogDependencyException(failedAccountDependencyException);
            }
            catch (Exception exception)
            {
                var failedAccountServiceException =
                    new FailedAccountServiceException(exception);

                throw CreateAndLogServiceException(failedAccountServiceException);
            }
        }

        private AccountValidationException CreateAndLogValidationException(Exception exception)
        {
            var accountValidationException = new AccountValidationException(exception, exception.Data);
            this.loggingBroker.LogError(accountValidationException);

            return accountValidationException;
        }

        private AccountDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var accountDependencyException = new AccountDependencyException(exception);
            this.loggingBroker.LogError(accountDependencyException);

            return accountDependencyException;
        }

        private AccountServiceException CreateAndLogServiceException(Exception exception)
        {
            var accountServiceException = new AccountServiceException(exception);
            this.loggingBroker.LogError(accountServiceException);

            return accountServiceException;
        }

        private int GetErrorCode(MongoWriteException ex)
        {
            return (ex.InnerException as MongoBulkWriteException)?.WriteErrors.FirstOrDefault()?.Code ?? 0;
        }
    }
}
