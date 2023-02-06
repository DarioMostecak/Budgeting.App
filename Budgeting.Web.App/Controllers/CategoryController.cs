using Budgeting.Web.App.Models.CategoryViews;
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
            var categoryViews = await this.manager.GetAllCategoriesAsync();
            return View(categoryViews);
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
            await manager.CreateCategoryAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async ValueTask<IActionResult> UpdateCategory([Bind("CategoryId", "Title", "Icon", "Type", "TimeCreated", "TimeModify")] CategoryView model)
        {
            await this.manager.UpdateCategoryAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Category/DeleteCategory/{categoryId}")]
        public async ValueTask<IActionResult> DeleteCategory(Guid categoryId)
        {
            await this.manager.DeleteCategoryAsync(categoryId);
            return RedirectToAction("Index");
        }


        private string SetFormViewData(CategoryView categoryView)
        {
            var categoryId = categoryView.CategoryId;
            return (categoryId == default) ? nameof(NewCategory) : nameof(UpdateCategory);
        }
    }
}
