namespace Budgeting.Web.App.Models.CategoryViews
{
    public class CategoryView
    {
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModify { get; set; }
    }
}
