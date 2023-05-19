// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Models.ExceptionModels;
using System.Collections;

namespace Budgeting.Web.App.Models.Categories.Exceptions
{
    public class CategoryValidationException : ExceptionModel
    {
        public CategoryValidationException(Exception innerException, IDictionary data)
            : base(message: innerException.Message, innerException, data)
        { }
    }
}
