// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.ExceptionModels;
using System.Collections;

namespace Budgeting.App.Api.Models.Users.Exceptions
{
    public class UserOrchestrationDependencyValidationException : ExceptionModel
    {
        public UserOrchestrationDependencyValidationException(Exception innerException, IDictionary data)
            : base(message: innerException.Message, innerException, data)
        { }
    }
}
