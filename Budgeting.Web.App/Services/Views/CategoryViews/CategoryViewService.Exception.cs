using Budgeting.Web.App.Models.Categories.Exceptions;
using Budgeting.Web.App.Models.CategoryViews;

namespace Budgeting.Web.App.Services.Views.CategoryViews
{
    public partial class CategoryViewService
    {
        private delegate ValueTask<CategoryView> ReturnigCategoryViewModelFunction();
        private delegate ValueTask<List<CategoryView>> ReturnigListCategoryViewModelsFunction();
        private delegate ValueTask ReturningNothingFunction();

        private async ValueTask<CategoryView> TryCatch(
            ReturnigCategoryViewModelFunction returnigCategoryViewModelFunction)
        {
            try
            {
                return await returnigCategoryViewModelFunction();
            }
            catch (CategoryValidationException)
            {
                throw;
            }
            catch (CategoryDependencyException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async ValueTask<List<CategoryView>> TryCatch(
            ReturnigListCategoryViewModelsFunction returnigListCategoryViewModelsFunction)
        {
            try
            {
                return await returnigListCategoryViewModelsFunction();
            }
            catch (CategoryDependencyException)
            {
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }

            catch (Exception)
            {
                throw;
            }
        }
    }
}
