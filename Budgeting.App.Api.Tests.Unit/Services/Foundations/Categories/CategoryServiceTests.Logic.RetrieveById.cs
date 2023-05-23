// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.App.Api.Models.Categories;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.App.Api.Tests.Unit.Services.Foundations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldRetreiveCategoryById()
        {
            //given
            Category randomCategory = CreateRandomCategory();
            Category storageCategory = randomCategory;
            Category expectedCategory = randomCategory.DeepClone();
            Guid categoryId = randomCategory.CategoryId;

            this.storageBrokerMock.Setup(broker =>
               broker.SelectCategoriesByIdAsync(categoryId))
                       .ReturnsAsync(storageCategory);

            //when
            Category actualCategory =
                await this.categoryService.RetriveCategoryByIdAsync(categoryId);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);

            this.storageBrokerMock.Verify(broker =>
               broker.SelectCategoriesByIdAsync(It.IsAny<Guid>()),
                 Times.Once());


            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
