using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.Models;
using Budgeting.App.Api.Models.Exceptions;
using MongoDB.Driver;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfMongoExceptionErrorOccursAndLogItAsync()
        {
            //given
            CategoryDto someCategoryDto = CreateRandomCategoryDto();
            MongoException mongoException = GetMongoException();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(mongoException);

            var expectedCategoryDependencyException =
                new CategoryDependencyException(failedCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()))
                .Throws(mongoException);

            //when
            ValueTask<CategoryDto> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(someCategoryDto);

            //then
            await Assert.ThrowsAsync<CategoryDependencyException>(() =>
                modifyCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCategoryDependencyException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()),
                  Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.UpdateCategoryAsync(It.IsAny<Category>()),
                  Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfExceptionOccursAndLogItAsync()
        {
            //given
            CategoryDto someCategoryDto = CreateRandomCategoryDto();
            var serviceException = new Exception();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(serviceException);

            var expectedCategoryServiceException =
                new CategoryServiceException(failedCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()))
                .Throws(serviceException);

            //when
            ValueTask<CategoryDto> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(someCategoryDto);

            //then
            await Assert.ThrowsAsync<CategoryServiceException>(() =>
                modifyCategoryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCategoryServiceException))),
                      Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()),
                  Times.Once);

            this.storageBrokerMock.Verify(broker =>
               broker.UpdateCategoryAsync(It.IsAny<Category>()),
                  Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

    }
}
