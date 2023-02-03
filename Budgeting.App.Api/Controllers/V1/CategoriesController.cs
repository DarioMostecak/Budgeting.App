using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.Services.ProcessingServices.CategoryProcessingServices;
using Microsoft.AspNetCore.Mvc;

namespace Budgeting.App.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route(ApiRoutes.Category.CategoryBaseRoute)]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryProcessingService categoryService;

        public CategoriesController(ICategoryProcessingService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var operationResult = this.categoryService.GetAllCategories();
            if (operationResult.IsError) return HandleErrorResponse(operationResult);

            return Ok(operationResult.Payload);
        }

        [HttpGet]
        [Route(ApiRoutes.Category.IdRoute)]
        public async ValueTask<IActionResult> GetCategoryById(Guid categoryId)
        {
            var operationResult = await this.categoryService.GetCategoryById(categoryId);
            if (operationResult.IsError) return HandleErrorResponse(operationResult);

            return Ok(operationResult.Payload);
        }

        [HttpPost]
        public async ValueTask<IActionResult> PostCategory([FromBody] CategoryDto categoryDto)
        {
            var operationResult = await this.categoryService.CreateCategoryAsync(categoryDto);
            if (operationResult.IsError) return HandleErrorResponse(operationResult);

            return CreatedAtAction(nameof(GetCategoryById),
                new { categoryId = operationResult.Payload.CategoryId },
                operationResult.Payload);
        }

        [HttpPut]
        public async ValueTask<IActionResult> PutCategory([FromBody] CategoryDto categoryDto)
        {
            var operationResult = await this.categoryService.UpdateCategoryAsync(categoryDto);
            if (operationResult.IsError) return HandleErrorResponse(operationResult);

            return Ok(operationResult.Payload);
        }

        [HttpDelete]
        [Route(ApiRoutes.Category.IdRoute)]
        public async ValueTask<IActionResult> DeleteCategory(Guid categoryId)
        {
            var operationResult = await this.categoryService.DeleteCategoryAsync(categoryId);
            if (operationResult.IsError) return HandleErrorResponse(operationResult);

            return Ok(operationResult.Payload);
        }
    }
}
