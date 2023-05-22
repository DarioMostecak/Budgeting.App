// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Models.Categories;
using Budgeting.Web.App.Models.Categories.Exceptions;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public partial class CategoryService
    {
        private void ValidateCategoryOnCreate(Category category)
        {
            ValidateCategoryIsNull(category);

            Validate(
                (rule: IsInvalidX(category.CategoryId), parameter: nameof(Category.CategoryId)),
                (rule: IsInvalidX(category.Title), parameter: nameof(Category.Title)),
                (rule: IsInvalidX(category.Type), parameter: nameof(Category.Type)),
                (rule: IsInvalidX(category.TimeCreated), parameter: nameof(Category.TimeCreated)),
                (rule: IsInvalidX(category.TimeModify), parameter: nameof(Category.TimeModify)));
        }

        private static dynamic IsInvalidX(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Value can't be null, white space or empty."
        };

        private static dynamic IsInvalidX(DateTime date) => new
        {
            Condition = date == default,
            Message = "Date is required."
        };

        private static dynamic IsInvalidX(Guid categoryId) => new
        {
            Condition = categoryId == Guid.Empty,
            Message = "Id isn't valid.",
        };

        private static void ValidateCategoryIsNull(Category category)
        {
            if (category is null) throw new NullCategoryException();
        }

        private static void Validate(params (dynamic rule, string parameter)[] validations)
        {
            var invalidCategorytException = new InvalidCategoryException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCategorytException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }

            }
            invalidCategorytException.ThrowIfContainsErrors();
        }
    }
}
