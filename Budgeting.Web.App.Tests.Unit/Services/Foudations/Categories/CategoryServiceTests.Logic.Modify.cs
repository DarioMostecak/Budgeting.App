// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Budgeting.Web.App.Models.Categories;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Budgeting.Web.App.Tests.Unit.Services.Foudations.Categories
{
    public partial class CategoryServiceTests
    {
        [Fact]
        public async Task ShouldModifyCategoryAsync()
        {
            //given
            DateTime randomDate = GetRandomDateTime();
            Category randomCategory = CreateRandomCategory();
            Category inputCategory = randomCategory;
            Category afterUpdateCategory = inputCategory;
            Category expectedCategory = inputCategory.DeepClone();


            this.apiBrokerMock.Setup(broker =>
               broker.PutCategoryAsync(It.IsAny<Category>()))
                       .ReturnsAsync(afterUpdateCategory);

            //when
            Category actualCategory =
                await this.categoryService.ModifyCategoryAsync(inputCategory);

            //then
            actualCategory.Should().BeEquivalentTo(expectedCategory);

            this.apiBrokerMock.Verify(broker =>
               broker.PutCategoryAsync(It.IsAny<Category>()),
                 Times.Once());

            this.apiBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
