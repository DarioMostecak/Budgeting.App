// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Categories.Exceptions
{
    public class CategoryDependencyException : ExceptionModel
    {
        public CategoryDependencyException(Exception innerException)
            : base(message: "Category dependency error occurred, contact support.", innerException)
        { }
    }
}
