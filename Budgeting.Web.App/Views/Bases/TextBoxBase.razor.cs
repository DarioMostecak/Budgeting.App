using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace Budgeting.Web.App.Views.Bases
{
    public partial class TextBoxBase : ComponentBase
    {
        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public ValidationAttribute ValidationAttribute { get; set; }

        [Parameter]
        public string HelperText { get; set; }

        [Parameter]
        public bool IsRequired { get; set; }

        [Parameter]
        public Variant Variant { get; set; }

        [Parameter]
        public bool Immediate { get; set; }

        [Parameter]
        public string RequiredError { get; set; }

        [Parameter]
        public InputType InputType { get; set; }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; }

        public bool IsEnabled => IsDisabled is false;

        public async Task SetValue(string value)
        {
            this.Value = value;
            await ValueChanged.InvokeAsync(this.Value);
        }

        private Task OnValueChanged(ChangeEventArgs changeEventArgs)
        {
            this.Value = changeEventArgs.Value.ToString();

            return ValueChanged.InvokeAsync(this.Value);
        }

        public void Disable()
        {
            this.IsDisabled = true;
            InvokeAsync(StateHasChanged);
        }

        public void Enable()
        {
            this.IsDisabled = false;
            InvokeAsync(StateHasChanged);
        }
    }
}
