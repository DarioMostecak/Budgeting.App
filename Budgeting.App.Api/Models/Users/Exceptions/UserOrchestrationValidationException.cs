// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.ExceptionModels;

namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class UserOrchestrationValidationException : ExceptionModel
    {
        public UserOrchestrationValidationException(Exception innerException)
            : base(message: innerException.Message, innerException)
        { }
    }
}
