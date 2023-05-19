// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Categories.Exceptions
{
    public class InvalidCategoryException : ExceptionModel
    {
        public InvalidCategoryException()
           : base(message: "Invalid category, validation error occurred, please fix errors and try again.")
        { }

        public InvalidCategoryException(Exception innerException) :
            base(message: "Invalid category data, please try again.", innerException)
        { }

        public InvalidCategoryException(string parameterName, object parameterValue)
            : base(message: $"Invalid category, " +
                  $"parameter name: {parameterName}, " +
                  $"parameter value: {parameterValue}.")
        { }
    }
}
