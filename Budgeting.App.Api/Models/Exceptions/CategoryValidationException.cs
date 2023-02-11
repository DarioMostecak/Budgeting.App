namespace Budgeting.App.Api.Models.Exceptions
{
    public class CategoryValidationException : Exception
    {
        public CategoryValidationException(Exception innerException)
            : base(message: "Invalid input, contact support.", innerException)
        { }

        public CategoryValidationException(InvalidCategoryException innerException)
            : base(message: "Invalid input, contact support.", innerException)
        {
            foreach (var error in innerException.ValidationErrors)
            {
                this.ValidationErrorMessages.Add(error.Item1 + ": " + error.Item2);
            }
        }

        public List<string> ValidationErrorMessages = new List<string>();
    }
}
