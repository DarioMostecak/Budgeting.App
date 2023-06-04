// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Accounts;
using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;

namespace Budgeting.App.Api.Services.Orchestrations.Users
{
    public partial class UserOrchestrationService
    {
        private static void ValidateUserIsNull(User user)
        {
            if (user is null)
                throw new NullUserException();
        }

        private static void ValidatePasswordIsNull(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new NullUserPasswordException();
        }

        private static void ValidateNewUserIsNull(User user)
        {
            if (user is null)
                throw new FailedOperationUserOrchestrationException();
        }

        private static void ValidateNewUserAccountIsNull(Account account)
        {
            if (account is null)
                throw new FailedOperationUserOrchestrationException();
        }
    }
}
