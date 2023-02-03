using Budgeting.Web.App.Contracts;
using Budgeting.Web.App.Models.Exceptions;
using System.Text;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public partial class CategoryService
    {
        private delegate ValueTask<CategoryViewModel> ReturnCategoryViewModelFunction();
        private delegate ValueTask<List<CategoryViewModel>> ReturnigCategoryViewModelsFunction();

        private async ValueTask<CategoryViewModel> TryCatch(
            ReturnCategoryViewModelFunction returnCategoryViewModelFunction)
        {
            try
            {
                return await returnCategoryViewModelFunction();
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
            catch (Exception exception)
            {
                var failedCategoryServiceException =
                    new FailedCategoryServiceException(exception);

                throw CreateAndLogServiceException(failedCategoryServiceException);
            }
        }

        private ValueTask<List<CategoryViewModel>> TryCatch(
            ReturnigCategoryViewModelsFunction returnigCategoryViewModelsFunction)
        {
            try
            {
                return returnigCategoryViewModelsFunction();
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

        private CategoryValidationException CreateAndLogValidationException(InvalidCategoryException invalidCategoryException)
        {
            var categoryValidationException = new CategoryValidationException(invalidCategoryException);

            var validationMessage = new StringBuilder();
            validationMessage.AppendLine(invalidCategoryException.Message);

            foreach (var validationErrors in invalidCategoryException.ValidationErrors)
            {
                validationMessage.AppendFormat("{0} : {1}\n", validationErrors.Item1, validationErrors.Item2);
            }

            this.loggingBroker.LogWarning(validationMessage.ToString());
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
