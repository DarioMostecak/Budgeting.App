using System.ComponentModel.DataAnnotations;

namespace Budgeting.Web.App.Filters
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ComparePasswordAttribute : ValidationAttribute
    {
        private readonly string? password;

        public ComparePasswordAttribute(string? password)
        {
            this.password = password;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != this.password)
                return new ValidationResult("Passwords are not the same");

            return ValidationResult.Success;

            //return base.IsValid(value, validationContext);
        }
    }
}
