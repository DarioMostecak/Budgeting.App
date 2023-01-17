using Budgeting.Web.App.Contracts;
using Budgeting.Web.App.OperationResults;
using Budgeting.Web.App.Services.ProcessingServices.CategoryProcessingServices;
using Microsoft.AspNetCore.Mvc;

namespace Budgeting.Web.App.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryProcessingService manager;

        public CategoryController(ICategoryProcessingService manager)
        {
            this.manager = manager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var operationResult = this.manager.GetAllCategories();
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
        public async ValueTask<IActionResult> UpdateCategory([Bind("CategoryId", "Title", "Type", "TimeCreated", "TimeModify")] CategoryViewModel model)
        {
            var operationResult = await this.manager.UpdateCategoryAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async ValueTask<IActionResult> DeleteCategory(Guid id)
        {
            var oprationResult = await this.manager.DeleteCategoryAsync(id);
            return RedirectToAction("Index");
        }

        //put in partial class category controller utilites
        private string SetFormViewData(OperationResult<CategoryViewModel> operationResult)
        {
            var categoryId = operationResult.Payload.CategoryId;
            return (categoryId == default) ? nameof(NewCategory) : nameof(UpdateCategory);
        }
    }
}
