// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.ExceptionModels;

namespace Budgeting.App.Api.Models.Accounts.Exceptions
{
    public class InvalidAccountException : ExceptionModel
    {
        public InvalidAccountException()
            : base(message: "Invalid account. Please fix errors and try again.")
        { }

        public InvalidAccountException(string parameterName, object parameterValue)
            : base(message: $"Invalid account, " +
                  $"parameter name: {parameterName}," +
                  $"parameter value: {parameterValue}")
        { }
    }
}
