// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.ExceptionModels;

namespace Budgeting.App.Api.Models.Accounts.Exceptions
{
    public class FailedAccountDependencyException : ExceptionModel
    {
        public FailedAccountDependencyException(Exception innerException)
            : base(message: "Failed dependency exception. Contact support.")
        { }
    }
}
