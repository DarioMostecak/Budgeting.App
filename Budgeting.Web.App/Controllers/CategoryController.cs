using Budgeting.Web.App.Models.CategoryViews;
using Budgeting.Web.App.Models.CategoryViews.Exceptions;
using Budgeting.Web.App.Services.Views.CategoryViews;
using Microsoft.AspNetCore.Mvc;

namespace Budgeting.Web.App.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryViewService manager;

        public CategoryController(ICategoryViewService manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public async ValueTask<IActionResult> Index()
        {
            try
            {
                var categoryViews = await this.manager.GetAllCategoriesAsync();
                return View(categoryViews);
            }
            catch (CategoryViewDependencyException)
            {
                return Redirect("error/500");
            }
            catch (CategoryViewServiceException)
            {
                return Redirect("error/500");
            }
            catch (Exception)
            {
                return Redirect("error/500");
            }
        }

        [HttpGet]
        [Route("Category/Form/{categoryId}")]
        public async ValueTask<IActionResult> Form(string categoryId)
        {
            var categoryView = await this.manager.GetCategoryById(categoryId);
            ViewData["ActionOperation"] = SetFormViewData(categoryView);
            return View(categoryView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async ValueTask<IActionResult> NewCategory([Bind("Title", "Icon", "Type")] CategoryView model)
        {
            try
            {
                await manager.CreateCategoryAsync(model);
                return RedirectToAction("Index");
            }
            catch (CategoryViewValidationException categoryViewValidationException)
               when (categoryViewValidationException.InnerException is NullCategoryViewException)
            {
                return Redirect("error/404");
            }
            catch (CategoryViewValidationException)
            {
                return Redirect("error/400");
            }
            catch (CategoryViewDependencyValidationException)
            {
                return Redirect("error/400");
            }
            catch (CategoryViewDependencyException)
            {
                return Redirect("error/500");
            }
            catch (CategoryViewServiceException)
            {
                return Redirect("error/500");
            }
            catch (Exception)
            {
                return Redirect("error/500");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async ValueTask<IActionResult> UpdateCategory([Bind("CategoryId", "Title", "Icon", "Type", "TimeCreated", "TimeModify")] CategoryView model)
        {
            try
            {
                await this.manager.UpdateCategoryAsync(model);
                return RedirectToAction("Index");
            }
            catch (CategoryViewValidationException categoryViewValidationException)
               when (categoryViewValidationException.InnerException is NullCategoryViewException)
            {
                return Redirect("error/404");
            }
            catch (CategoryViewValidationException)
            {
                return Redirect("error/400");
            }
            catch (CategoryViewDependencyValidationException)
            {
                return Redirect("error/400");
            }
            catch (CategoryViewDependencyException)
            {
                return Redirect("error/500");
            }
            catch (CategoryViewServiceException)
            {
                return Redirect("error/500");
            }
        }

        [HttpGet]
        [Route("Category/DeleteCategory/{categoryId}")]
        public async ValueTask<IActionResult> DeleteCategory(Guid categoryId)
        {
            try
            {
                await this.manager.DeleteCategoryAsync(categoryId);
                return RedirectToAction("Index");
            }
            catch (CategoryViewValidationException categoryViewValidationException)
               when (categoryViewValidationException.InnerException is NullCategoryViewException)
            {
                return Redirect("error/404");
            }
            catch (CategoryViewValidationException)
            {
                return Redirect("error/400");
            }
            catch (CategoryViewDependencyValidationException)
            {
                return Redirect("error/400");
            }
            catch (CategoryViewDependencyException)
            {
                return Redirect("error/500");
            }
            catch (CategoryViewServiceException)
            {
                return Redirect("error/500");
            }

        }


        private string SetFormViewData(CategoryView categoryView)
        {
            var categoryId = categoryView.CategoryId;
            return (categoryId == default) ? nameof(NewCategory) : nameof(UpdateCategory);
        }
    }
}
