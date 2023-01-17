using Budgeting.Web.App.Contracts;
using Budgeting.Web.App.Models.Exceptions;
using MongoDB.Driver;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public partial class CategoryService
    {
        private delegate ValueTask<CategoryViewModel> ReturnCategoryFunction();
        private delegate IQueryable<CategoryViewModel> ReturnigQueryableCategoryFunction();

        private async ValueTask<CategoryViewModel> TryCatch(
            ReturnCategoryFunction returnAssignmentFunction)
        {
            try
            {
                return await returnAssignmentFunction();
            }
            catch (NullCategoryException nullCategoryException)
            {
                throw CreateAndLogValidationException(nullCategoryException);
            }
            catch (InvalidCategoryException invalidInputException)
            {
                throw CreateAndLogValidationException(invalidInputException);
            }
            catch (NotFoundCategoryException notFoundCategoryException)
            {
                throw CreateAndLogValidationException(notFoundCategoryException);
            }
            catch (MongoDuplicateKeyException mongoDuplicateKeyException)
            {
                var alreadyExistsCategoryException =
                    new AlreadyExistsCategoryException(mongoDuplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCategoryException);
            }
            catch (MongoException mongoException)
            {
                throw CreateAndLogCriticalDependencyException(mongoException);
            }
            catch (Exception exception)
            {
                var failedCategoryServiceException =
                    new FailedCategoryServiceException(exception);

                throw CreateAndLogServiceException(failedCategoryServiceException);
            }
        }

        private IQueryable<CategoryViewModel> TryCatch(
            ReturnigQueryableCategoryFunction returnigQueryableCategoryFunction)
        {
            try
            {
                return returnigQueryableCategoryFunction();
            }
            catch (MongoException mongoException)
            {
                throw CreateAndLogCriticalDependencyException(mongoException);
            }
            catch (Exception exception)
            {
                var failedCategoryServiceException =
                    new FailedCategoryServiceException(exception);

                throw CreateAndLogServiceException(failedCategoryServiceException);
            }
        }

        private CategoryValidationException CreateAndLogValidationException(Exception exception)
        {
            var categoryValidationException = new CategoryValidationException(exception);
            this.loggingBroker.LogError(categoryValidationException);

            return categoryValidationException;
        }

        private CategoryDependencyException CreateAndLogCriticalDependencyException(Exception exception)
        {
            var categoryDependencyException = new CategoryDependencyException(exception);
            this.loggingBroker.LogCritical(categoryDependencyException);

            return categoryDependencyException;
        }

        private CategoryServiceException CreateAndLogServiceException(Exception exception)
        {
            var categoryServiceException = new CategoryServiceException(exception);
            this.loggingBroker.LogError(categoryServiceException);

            return categoryServiceException;
        }
    }
}
