// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Models.Categories;
using Budgeting.Web.App.Models.Categories.Exceptions;
using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Services.Foundations.Categories
{
    public partial class CategoryService
    {
        private delegate ValueTask<Category> CategoryReturnigFunction();
        private delegate ValueTask<IEnumerable<Category>> CategoriesReturnigFunction();

        private async ValueTask<Category> TryCatch(
            CategoryReturnigFunction categoryReturnigFunction)
        {
            try
            {
                return await categoryReturnigFunction();
            }
            catch (NullCategoryException nullCategoryException)
            {
                throw CreateAndLogValidationException(nullCategoryException);
            }
            catch (InvalidCategoryException invalidCategoryException)
            {
                throw CreateAndLogValidationException(invalidCategoryException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizeException)
            {
                var failedCategoryUnauthorizedException =
                    new FailedCategoryUnauthorizedException(httpResponseUnauthorizeException);

                throw CreateAndLogUnauthorizedException(failedCategoryUnauthorizedException);
            }
            catch (HttpResponseNotFoundException httpResponseNotFoundException)
            {
                var failCategoryDependencyException =
                    new FailedCategoryDependencyException(httpResponseNotFoundException);

                throw CreateAndLogDependencyValidationException(failCategoryDependencyException);
            }
            catch (HttpResponseBadRequestException httpResponseBadRequestException)
            {
                var invalidCategoryException =
                    new InvalidCategoryException(httpResponseBadRequestException);

                throw CreateAndLogDependencyValidationException(invalidCategoryException);
            }
            catch (HttpResponseConflictException httpResponseConflictException)
            {
                var alreadyExistsCategoryException =
                    new AlreadyExistsCategoryException(httpResponseConflictException);

                throw CreateAndLogDependencyValidationException(alreadyExistsCategoryException);
            }
            catch (HttpResponseInternalServerErrorException httpResponseInternalserverError)
            {
                var failedCategoryDependencyException =
                    new FailedCategoryDependencyException(httpResponseInternalserverError);

                throw CreateAndLogDependencyException(failedCategoryDependencyException);
            }
            catch (HttpRequestException httpRequestException)
            {
                var failedCategoryDependencyException =
                    new FailedCategoryDependencyException(httpRequestException);

                throw CreateAndLogDependencyException(failedCategoryDependencyException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedCategoryDependencyEception =
                    new FailedCategoryDependencyException(httpResponseException);

                throw CreateAndLogDependencyException(failedCategoryDependencyEception);
            }
            catch (Exception serviceException)
            {
                var failedCategoryServiceException =
                    new FailedCategoryDependencyException(serviceException);

                throw CreateAndLogServiceException(failedCategoryServiceException);
            }
        }

        private async ValueTask<IEnumerable<Category>> TryCatch(
            CategoriesReturnigFunction categoriesReturnigFunction)
        {
            try
            {
                return await categoriesReturnigFunction();
            }
            catch (HttpRequestException httpRequestException)
            {
                var failedCategoryDependencyException =
                    new FailedCategoryDependencyException(httpRequestException);

                throw CreateAndLogDependencyException(failedCategoryDependencyException);
            }
            catch (HttpResponseUnauthorizedException httpResponseUnauthorizeException)
            {
                var failedCategoryUnauthorizedException =
                    new FailedCategoryUnauthorizedException(httpResponseUnauthorizeException);

                throw CreateAndLogUnauthorizedException(failedCategoryUnauthorizedException);
            }
            catch (HttpResponseException httpResponseException)
            {
                var failedCategoryDependencyEception =
                    new FailedCategoryDependencyException(httpResponseException);

                throw CreateAndLogDependencyException(failedCategoryDependencyEception);
            }
            catch (Exception serviceException)
            {
                var failedCategoryServiceException =
                    new FailedCategoryServiceException(serviceException);

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

        private CategoryUnauthorizedException CreateAndLogUnauthorizedException(Exception exception)
        {
            var categoryUnauthorizedException = new CategoryUnauthorizedException(exception);
            this.loggingBroker.LogError(categoryUnauthorizedException);

            return categoryUnauthorizedException;
        }

        private CategoryDependencyValidationException CreateAndLogDependencyValidationException(Exception exception)
        {
            var categoryDependencyValidationException = new CategoryDependencyValidationException(exception);
            this.loggingBroker.LogError(categoryDependencyValidationException);

            return categoryDependencyValidationException;
        }
    }
}
