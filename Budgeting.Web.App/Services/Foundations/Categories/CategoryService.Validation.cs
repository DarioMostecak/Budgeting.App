using Budgeting.Web.App.Models;
using Budgeting.Web.App.Models.Exceptions;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public partial class CategoryService
    {
        private void ValidateCategoryOnCreate(Category category)
        {
            ValidateCategoryIsNull(category);

            Validate(
                (rule: IsInvalidX(category.Title), parameter: nameof(Category.Title)),
                (rule: IsInvalidX(category.TimeCreated), parameter: nameof(Category.TimeCreated)),
                (rule: IsInvalidX(category.TimeModify), parameter: nameof(Category.TimeModify)));
        }

        private void ValidateCategoryOnModify(Category category)
        {
            ValidateCategoryIsNull(category);

            Validate(
                (rule: IsInvalidX(category.CategoryId), parameter: nameof(category.CategoryId)),
                (rule: IsInvalidX(category.Title), parameter: nameof(category.Title)),
                (rule: IsInvalidX(category.TimeCreated), parameter: nameof(category.TimeCreated)),
                (rule: IsInvalidX(category.TimeModify), parameter: nameof(category.TimeModify)));
        }


        private static dynamic IsInvalidX(Guid categoryId) => new
        {
            Condition = categoryId == Guid.Empty,
            Message = "Id isn't valid.",
        };

        private static dynamic IsInvalidX(string text) => new
        {
            Condition = String.IsNullOrWhiteSpace(text) || (text.Length >= 20 || text.Length <= 3),
            Message = "Category title isn't valid"

        };

        private static dynamic IsInvalidX(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        //remove this method
        private static dynamic IsNotSame(
            Guid firstId,
            Guid secondId,
            string secondIdName) => new
            {
                Condition = firstId != secondId,
                Message = $"Id is not the same as {secondIdName}"
            };


        private static bool IsInvalid(Guid input) => input == default;

        private static void ValidateCategoryIsNull(Category category)
        {
            if (category is null) throw new NullCategoryException();
        }

        private static void ValidateCategoryIdIsNull(Guid categoryId)
        {
            if (IsInvalid(categoryId))
            {
                var invalidCategorytException = new InvalidCategoryException(nameof(Category.CategoryId), categoryId);
                throw invalidCategorytException;
            }
        }

        private static void ValidateStorageCategory(Category storageCategory,
            Guid categoryId)
        {
            if (storageCategory is null) throw new NotFoundCategoryException(categoryId);
        }

        private static void ValidateAgainstStorageCategoryOnModify(
            Category inputCategory,
            Category storageCategory)
        {
            switch (inputCategory)
            {
                case { } when inputCategory.CategoryId != storageCategory.CategoryId:
                    throw new InvalidCategoryException(
                        parameterId: inputCategory.CategoryId);

                case { } when Math.Abs((inputCategory.TimeCreated - storageCategory.TimeCreated).TotalSeconds) >= 1:
                    throw new InvalidCategoryException(
                        parameterName: nameof(Category.TimeCreated),
                        parameterValue: inputCategory.TimeCreated);

                case { } when Math.Abs((inputCategory.TimeModify - storageCategory.TimeModify).TotalSeconds) <= 1:
                    throw new InvalidCategoryException(
                        parameterName: nameof(Category.TimeModify),
                        parameterValue: inputCategory.TimeModify);
            }
        }



        private static void Validate(params (dynamic rule, string parameter)[] validations)
        {
            var invalidCategorytException = new InvalidCategoryException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidCategorytException.ValidationErrors.Add((parameter, rule.Message));
                }
            }

            if (invalidCategorytException.ValidationErrors.Count > 0) throw invalidCategorytException;
        }

    }
}
