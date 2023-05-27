// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.ExceptionModels;

namespace Budgeting.App.Api.Models.Accounts.Exceptions
{
    public class AccountDependencyException : ExceptionModel
    {
        public AccountDependencyException(Exception innerException)
            : base(message: "Account dependency error occurred, please contact support.", innerException)
        { }
    }
}
