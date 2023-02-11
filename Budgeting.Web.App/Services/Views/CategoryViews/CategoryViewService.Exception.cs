using Budgeting.Web.App.Models.Categories.Exceptions;
using Budgeting.Web.App.Models.CategoryViews;
using Budgeting.Web.App.Models.CategoryViews.Exceptions;

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
            catch (NullCategoryViewException nullCategoryViewException)
            {
                throw CreateAndLogValidationException(nullCategoryViewException);
            }
            catch (InvalidCategoryViewException invalidCategoryViewException)
            {
                throw CreateAndLogValidationException(invalidCategoryViewException);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                throw CreateAndLogDependencyValidationException(categoryValidationException);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                throw CreateAndLogDependencyException(categoryDependencyException);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                throw CreateAndLogServiceException(categoryServiceException);
            }
            catch (Exception exception)
            {
                var failedCategoryViewServiceException =
                    new FailedCategoryViewServiceException(exception);

                throw CreateAndLogServiceException(failedCategoryViewServiceException);
            }
        }

        private async ValueTask<List<CategoryView>> TryCatch(
            ReturnigListCategoryViewModelsFunction returnigListCategoryViewModelsFunction)
        {
            try
            {
                return await returnigListCategoryViewModelsFunction();
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                throw CreateAndLogDependencyException(categoryDependencyException);
            }
            catch (CategoryServiceException categoryServiceexception)
            {
                throw CreateAndLogServiceException(categoryServiceexception);
            }
            catch (Exception exception)
            {
                var failedCategoryViewServiceException =
                   new FailedCategoryViewServiceException(exception);

                throw CreateAndLogServiceException(failedCategoryViewServiceException);
            }
        }

        private async ValueTask TryCatch(ReturningNothingFunction returningNothingFunction)
        {
            try
            {
                await returningNothingFunction();
            }
            catch (NullCategoryViewException nullCategoryViewException)
            {
                throw CreateAndLogValidationException(nullCategoryViewException);
            }
            catch (InvalidCategoryViewException invalidCategoryViewException)
            {
                throw CreateAndLogValidationException(invalidCategoryViewException);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                throw CreateAndLogDependencyValidationException(categoryValidationException);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                throw CreateAndLogDependencyException(categoryDependencyException);
            }
            catch (CategoryServiceException categoryServiceexception)
            {
                throw CreateAndLogServiceException(categoryServiceexception);
            }
            catch (Exception exception)
            {
                var failedCategoryViewServiceException =
                   new FailedCategoryViewServiceException(exception);

                throw CreateAndLogServiceException(failedCategoryViewServiceException);
            }
        }

        private CategoryViewValidationException CreateAndLogValidationException(Exception exception)
        {
            var categoryViewValidationException =
                new CategoryViewValidationException(exception);

            this.loggingBroker.LogError(categoryViewValidationException);

            return categoryViewValidationException;
        }

        private CategoryViewDependencyValidationException CreateAndLogDependencyValidationException(Exception exception)
        {
            var categoryViewDependencyValidationException =
                new CategoryViewDependencyValidationException(exception);

            this.loggingBroker.LogError(categoryViewDependencyValidationException);

            return categoryViewDependencyValidationException;
        }

        private CategoryViewDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var categoryViewDependencyException =
                new CategoryViewDependencyException(exception);

            this.loggingBroker.LogError(categoryViewDependencyException);

            return categoryViewDependencyException;
        }

        private CategoryViewServiceException CreateAndLogServiceException(Exception exception)
        {
            var categoryViewServiceException =
                new CategoryViewServiceException(exception);

            this.loggingBroker.LogError(categoryViewServiceException);

            return categoryViewServiceException;
        }
    }
}
