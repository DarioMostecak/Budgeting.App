using Budgeting.App.Api.Models.Categories;
using Budgeting.App.Api.Models.Categories.Exceptions;
using Budgeting.App.Api.Services.Foundations.Categories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budgeting.App.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoriesController : BaseController
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
                IQueryable categories =
                    this.categoryService.RetrieveAllCategories();

                return Ok(categories);
            }
            catch (CategoryDependencyException categoryDependencyExceptionn)
            {
                return InternalServerError(categoryDependencyExceptionn);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return InternalServerError(categoryServiceException);
            }
        }

        [HttpGet]
        [Route("{categoryId}")]
        public async ValueTask<IActionResult> GetCategoryByIdAsync(Guid categoryId)
        {
            try
            {
                Category category =
                    await this.categoryService.RetriveCategoryByIdAsync(categoryId);

                return Ok(category);
            }
            catch (CategoryValidationException categoryValidationException)
               when (categoryValidationException.InnerException is NotFoundCategoryException)
            {
                return NotFound(categoryValidationException.InnerException);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                return BadRequest(categoryValidationException);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return InternalServerError(categoryDependencyException);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return InternalServerError(categoryServiceException);
            }


        }

        [HttpPost]
        public async ValueTask<IActionResult> PostCategoryAsync([FromBody] Category category)
        {
            try
            {
                Category createdCategory =
                await this.categoryService.AddCategoryAsync(category);

                return Created(createdCategory);
            }
            catch (CategoryValidationException categoryValidationException)
              when (categoryValidationException.InnerException is AlreadyExistsCategoryException)
            {
                return Conflict(categoryValidationException.InnerException);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                return BadRequest(categoryValidationException);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return InternalServerError(categoryDependencyException);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return InternalServerError(categoryServiceException);
            }

        }

        [HttpPut]
        public async ValueTask<IActionResult> PutCategoryAsync([FromBody] Category category)
        {
            try
            {
                Category updatedCategory =
                    await this.categoryService.ModifyCategoryAsync(category);

                return Ok(updatedCategory);
            }
            catch (CategoryValidationException categoryValidationException)
               when (categoryValidationException.InnerException is NotFoundCategoryException)
            {
                return NotFound(categoryValidationException.InnerException);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                return BadRequest(categoryValidationException);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return InternalServerError(categoryDependencyException);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return InternalServerError(categoryServiceException);
            }

        }

        [HttpDelete]
        [Route("{categoryId}")]
        public async ValueTask<IActionResult> DeleteCategoryAsync(Guid categoryId)
        {
            try
            {
                Category deletedCategory =
                    await this.categoryService.RemoveCategoryByIdAsync(categoryId);

                return Ok(deletedCategory);
            }
            catch (CategoryValidationException categoryValidationException)
               when (categoryValidationException.InnerException is NotFoundCategoryException)
            {
                return NotFound(categoryValidationException.InnerException);
            }
            catch (CategoryValidationException categoryValidationException)
            {
                return BadRequest(categoryValidationException);
            }
            catch (CategoryDependencyException categoryDependencyException)
            {
                return InternalServerError(categoryDependencyException);
            }
            catch (CategoryServiceException categoryServiceException)
            {
                return InternalServerError(categoryServiceException);
            }
        }
    }
}
