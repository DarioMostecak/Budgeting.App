using Budgeting.Web.App.Contracts;
using Budgeting.Web.App.OperationResults;
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
            var operationResult = await this.manager.GetAllCategoriesAsync();
            return View(operationResult.Payload);
        }

        [HttpGet]
        [Route("Category/Form/{categoryId}")]
        public async ValueTask<IActionResult> Form(string categoryId)
        {
            var operationResult = await this.manager.GetCategoryById(categoryId);
            ViewData["ActionOperation"] = SetFormViewData(operationResult);
            return View(operationResult.Payload);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async ValueTask<IActionResult> NewCategory([Bind("Title", "Icon", "Type")] CategoryViewModel model)
        {
            var operationResult = await manager.CreateCategoryAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async ValueTask<IActionResult> UpdateCategory([Bind("CategoryId", "Title", "Icon", "Type", "TimeCreated", "TimeModify")] CategoryViewModel model)
        {
            var operationResult = await this.manager.UpdateCategoryAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Category/DeleteCategory/{categoryId}")]
        public async ValueTask<IActionResult> DeleteCategory(Guid categoryId)
        {
            var oprationResult = await this.manager.DeleteCategoryAsync(categoryId);
            return RedirectToAction("Index");
        }


        private string SetFormViewData(OperationResult<CategoryViewModel> operationResult)
        {
            var categoryId = operationResult.Payload.CategoryId;
            return (categoryId == default) ? nameof(NewCategory) : nameof(UpdateCategory);
        }
    }
}
