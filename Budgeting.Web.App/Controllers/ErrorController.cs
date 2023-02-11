using Budgeting.Web.App.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Budgeting.Web.App.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        [Route("500")]
        public IActionResult AppError()
        {
            var error = new ErrorView();
            error.StatusCode = 500;
            error.Message = "Server Error. Contact support!";

            return View("Error", error);
        }

        [Route("400")]
        public IActionResult BadRequesrError()
        {
            var error = new ErrorView();
            error.StatusCode = 400;
            error.Message = "Bad Request. Contact support!";

            return View("Error", error);
        }

        [Route("404")]
        public IActionResult NotFoundError()
        {
            var error = new ErrorView();
            error.StatusCode = 404;
            error.Message = "Not Found. Contact support!";

            return View("Error", error);
        }
    }
}
