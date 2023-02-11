using Budgeting.Web.App.Models.CategoryViews;
using Budgeting.Web.App.Models.CategoryViews.Exceptions;

namespace Budgeting.Web.App.Services.Views.CategoryViews
{
    public partial class CategoryViewService
    {

        private static void ValidateCategoryViewIsNull(CategoryView categoryView)
        {
            if (categoryView is null)
                throw new NullCategoryViewException();
        }

        private static void IsGuidValid(string categoryId)
        {
            if (!Guid.TryParse(categoryId, out _))
                throw new InvalidCategoryViewException(
                    parameterName: nameof(CategoryView.CategoryId),
                    parameterValue: categoryId);
        }

        private static void IsGuidDefault(Guid categoryId)
        {
            if (categoryId == default)
                throw new InvalidCategoryViewException(
                    parameterName: nameof(CategoryView.CategoryId),
                    parameterValue: categoryId);
        }

        private static bool IsGuidDefault(string categoryId)
        {
            var id = Guid.Parse(categoryId);

            return id == default;
        }
    }
}
