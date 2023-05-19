// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

namespace Budgeting.App.Api.Models.Categories.Exceptions
{
    public class NotFoundCategoryException : Exception
    {
        public NotFoundCategoryException(Guid categoryId)
            : base(message: string.Format($"Couldn't find category with id: {categoryId}.")) { }

    }
}
