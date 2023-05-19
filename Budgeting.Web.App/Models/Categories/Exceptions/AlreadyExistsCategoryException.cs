using Budgeting.Web.App.Models.ExceptionModels;

namespace Budgeting.Web.App.Models.Categories.Exceptions
{
    public class AlreadyExistsCategoryException : ExceptionModel
    {
        public AlreadyExistsCategoryException()
            : base(message: "Category already exist.") { }
    }
}
