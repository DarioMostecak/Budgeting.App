using Budgeting.App.Api.Contracts;
using Budgeting.App.Api.Models.Exceptions;
using Budgeting.App.Api.Services.Foundations.Categories;
using Microsoft.AspNetCore.Mvc;

namespace Budgeting.App.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            try
            {
                IQueryable categoryDtos =
                    this.categoryService.RetrieveAllCategories();

                return Ok(categoryDtos);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                return Problem(categoryValidationException.Message);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return Problem(categoryServiceException.Message);
            }
        }

        [HttpGet]
        [Route("{categoryId}")]
        public async ValueTask<IActionResult> GetCategoryById(Guid categoryId)
        {
            try
            {
                CategoryDto categoryDto =
                    await this.categoryService.RetriveCategoryByIdAsync(categoryId);

                return Ok(categoryDto);
            }
            catch (CategoryValidationException categoryValidationException)
               when (categoryValidationException.InnerException is NotFoundCategoryException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);
                return NotFound(innerMessage);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);
                return BadRequest(innerMessage);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return Problem(categoryDependencyException.Message);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return Problem(categoryServiceException.Message);
            }


        }

        [HttpPost]
        public async ValueTask<IActionResult> PostCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                CategoryDto createdCategoryDto =
                await this.categoryService.AddCategoryAsync(categoryDto);

                return CreatedAtAction(nameof(GetCategoryById)
                    , new { categoryId = categoryDto.CategoryId }, categoryDto);
            }
            catch (CategoryValidationException categoryValidationException)
              when (categoryValidationException.InnerException is AlreadyExistsCategoryException)
            {
                return Conflict(categoryValidationException.InnerException.Message);
            }
            catch (CategoryValidationException categoryValidationException)
              when (categoryValidationException.ValidationErrorMessages.Count > 0)
            {
                var errorResponse = new ErrorResponse();
                errorResponse.Message = categoryValidationException.Message;
                foreach (var error in categoryValidationException.ValidationErrorMessages)
                {
                    errorResponse.Errors.Add(error);
                }
                return BadRequest(errorResponse);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, categoryDependencyException.Message);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, categoryServiceException.Message);
            }

        }

        [HttpPut]
        public async ValueTask<IActionResult> PutCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                CategoryDto updatedCategoryDto =
                    await this.categoryService.ModifyCategoryAsync(categoryDto);

                return Ok(updatedCategoryDto);
            }
            catch (CategoryValidationException categoryValidationException)
               when (categoryValidationException.InnerException is NotFoundCategoryException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);
                return NotFound(innerMessage);
            }
            catch (CategoryValidationException categoryValidationException)
              when (categoryValidationException.ValidationErrorMessages.Count > 0)
            {
                var errorResponse = new ErrorResponse();
                errorResponse.Message = categoryValidationException.Message;
                foreach (var error in categoryValidationException.ValidationErrorMessages)
                {
                    errorResponse.Errors.Add(error);
                }
                return BadRequest(errorResponse);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);
                return BadRequest(innerMessage);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return StatusCode(500, categoryDependencyException.Message);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return StatusCode(500, categoryServiceException.Message);
            }

        }

        [HttpDelete]
        [Route("{categoryId}")]
        public async ValueTask<IActionResult> DeleteCategory(Guid categoryId)
        {
            try
            {
                CategoryDto deletedCategoryDto =
                    await this.categoryService.RemoveCategoryByIdAsync(categoryId);

                return Ok(deletedCategoryDto);
            }
            catch (CategoryValidationException categoryValidationException)
               when (categoryValidationException.InnerException is NotFoundCategoryException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);
                return NotFound(innerMessage);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                string innerMessage = GetInnerMessage(categoryValidationException);
                return BadRequest(innerMessage);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return StatusCode(500, categoryDependencyException.Message);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return StatusCode(500, categoryServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
