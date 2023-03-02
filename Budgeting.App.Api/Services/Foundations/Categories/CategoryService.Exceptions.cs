using Budgeting.App.Api.Models.Categories;
using Budgeting.App.Api.Models.Categories.Exceptions;
using MongoDB.Driver;

namespace Budgeting.App.Api.Services.Foundations.Categories
{
    public partial class CategoryService
    {
        private delegate ValueTask<Category> ReturnCategoryFunction();
        private delegate IQueryable<Category> ReturnigQueryableCategoriesFunction();

        private async ValueTask<Category> TryCatch(
            ReturnCategoryFunction returnCategoryFunction)
        {
            try
            {
                return await returnCategoryFunction();
            }
            catch (NullCategoryException nullCategoryException)
            {
                throw CreateAndLogValidationException(nullCategoryException);
            }
            catch (InvalidCategoryException invalidCategoryException)
            {
                throw CreateAndLogValidationException(invalidCategoryException);
            }
            catch (NotFoundCategoryException notFoundCategoryException)
            {
                throw CreateAndLogValidationException(notFoundCategoryException);
            }
            catch (MongoWriteException mongoDuplicateKeyException)
            {
                var alreadyExistsCategoryException =
                    new AlreadyExistsCategoryException(mongoDuplicateKeyException);

                throw CreateAndLogValidationException(alreadyExistsCategoryException);
            }
            catch (MongoException mongoException)
            {
                var failedCategoryServiceException =
                    new FailedCategoryServiceException(mongoException);

                throw CreateAndLogDependencyException(failedCategoryServiceException);
            }
            catch (Exception exception)
            {
                var failedCategoryServiceException =
                    new FailedCategoryServiceException(exception);

                throw CreateAndLogServiceException(failedCategoryServiceException);
            }
        }

        private IQueryable<Category> TryCatch(
            ReturnigQueryableCategoriesFunction returnigQueryableCategoriesFunction)
        {
            try
            {
                return returnigQueryableCategoriesFunction();
            }
            catch (MongoException mongoException)
            {
                throw CreateAndLogDependencyException(mongoException);
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
            var categoryValidationException = new CategoryValidationException(exception, exception.Data);
            this.loggingBroker.LogError(categoryValidationException);

            return categoryValidationException;
        }

        private CategoryDependencyException CreateAndLogDependencyException(Exception exception)
        {
            var categoryDependencyException = new CategoryDependencyException(exception);
            this.loggingBroker.LogError(categoryDependencyException);

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
