using Budgeting.Web.App.Models.ContainerComponents;
using Budgeting.Web.App.Views.Bases;
using Microsoft.AspNetCore.Components;


namespace Budgeting.Web.App.Views.Components.TransactionComponents
{
    public partial class TrasactionFormComponent : ComponentBase
    {
        public ComponentState State { get; set; }
        public TextBase? TrasactionFormTitleText { get; set; }
        public DecimalBoxBase? AmountDecimalBox { get; set; }
        public TextBoxBase? CategoryNameTextBox { get; set; }
        public ButtonBase? SubmitButton { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }

        public string SelectedOptions { get; set; }
    }
}
