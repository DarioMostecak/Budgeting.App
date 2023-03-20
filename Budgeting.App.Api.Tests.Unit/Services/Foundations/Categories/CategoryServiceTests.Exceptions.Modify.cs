using Budgeting.App.Api.Models.Categories;
using Budgeting.App.Api.Models.Categories.Exceptions;
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
            Category someCategory = CreateRandomCategory();
            MongoException mongoException = GetMongoException();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(mongoException);

            var expectedCategoryDependencyException =
                new CategoryDependencyException(failedCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()))
                .ThrowsAsync(mongoException);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(someCategory);

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
            Category someCategory = CreateRandomCategory();
            var serviceException = new Exception();

            var failedCategoryServiceException =
                new FailedCategoryServiceException(serviceException);

            var expectedCategoryServiceException =
                new CategoryServiceException(failedCategoryServiceException);

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()))
                .ThrowsAsync(serviceException);

            //when
            ValueTask<Category> modifyCategoryTask =
                this.categoryService.ModifyCategoryAsync(someCategory);

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
