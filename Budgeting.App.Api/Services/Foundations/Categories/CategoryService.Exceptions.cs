using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.Models.Exceptions;
using MongoDB.Driver;

namespace Budgeting.App.Api.Services.Foundations.Categories
{
    public partial class CategoryService
    {
        private delegate ValueTask<CategoryDto> ReturnCategoryDtoFunction();
        private delegate IQueryable<CategoryDto> ReturnigQueryableCategoryDtosFunction();

        private async ValueTask<CategoryDto> TryCatch(
            ReturnCategoryDtoFunction returnCategoryDtoFunction)
        {
            try
            {
                return await returnCategoryDtoFunction();
            }
            catch (NullCategoryException nullCategoryException)
            {
                throw CreateAndLogValidationException(nullCategoryException);
            }
            catch (InvalidCategoryException invalidCategoryException)
               when (invalidCategoryException.ValidationErrors.Count != 0)
            {
                throw CreateAndLogValidationException(invalidCategoryException);
            }
            catch (InvalidCategoryException invalidCategoryException)
            {
                throw CreateAndLogValidationException(invalidCategoryException as Exception);
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

        private IQueryable<CategoryDto> TryCatch(
            ReturnigQueryableCategoryDtosFunction returnigQueryableCategoryDtosFunction)
        {
            try
            {
                return returnigQueryableCategoryDtosFunction();
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

        //see to add that validation errors are writen on console 
        private CategoryValidationException CreateAndLogValidationException(InvalidCategoryException invalidCategoryException)
        {
            var categoryValidationException = new CategoryValidationException(invalidCategoryException);
            AddErrorMessages(invalidCategoryException.ValidationErrors, categoryValidationException.ValidationErrorMessages);

            this.loggingBroker.LogError(categoryValidationException);

            return categoryValidationException;
        }

        private void AddErrorMessages(List<(string, string)> listValidationErrors, List<string> errors)
        {
            foreach (var validationError in listValidationErrors)
            {
                errors.Add(validationError.Item1 + ": " + validationError.Item2);
            }
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
