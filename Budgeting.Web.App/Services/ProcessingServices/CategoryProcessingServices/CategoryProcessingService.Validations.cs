namespace Budgeting.Web.App.Services.ProcessingServices.CategoryProcessingServices
{
    public partial class CategoryProcessingService
    {
        public bool IsGuidId(string categoryId)
        {
            var validationResult = Guid.TryParse(categoryId, out var id);

            return (validationResult && id != Guid.Empty);
        }
    }
}
