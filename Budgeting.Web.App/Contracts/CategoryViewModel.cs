using Budgeting.Web.App.Models;

namespace Budgeting.Web.App.Contracts
{
    public class CategoryViewModel
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModify { get; set; }

        public static explicit operator CategoryViewModel(Category category)
        {
            return new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                Title = category.Title,
                Icon = category.Icon,
                Type = category.Type,
                TimeCreated = category.TimeCreated,
                TimeModify = category.TimeModify
            };
        }
    }
}
