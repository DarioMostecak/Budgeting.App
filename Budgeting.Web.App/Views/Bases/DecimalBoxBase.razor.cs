// ---------------------------------------------------------------
// Author: Dario Mostecak
// Copyright (c) 2023 Dario Mostecak. All rights reserved.
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.ComponentModel.DataAnnotations;

namespace Budgeting.Web.App.Views.Bases
{
    public partial class DecimalBoxBase : ComponentBase
    {
        [Parameter]
        public decimal Value { get; set; }

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
        public EventCallback<decimal> ValueChanged { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; }

        public bool IsEnabled => IsDisabled is false;

        public async Task SetValue(decimal value)
        {
            this.Value = value;
            await ValueChanged.InvokeAsync(this.Value);
        }

        private Task OnValueChanged(ChangeEventArgs changeEventArgs)
        {
            this.Value = Convert.ToDecimal(changeEventArgs.Value);

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
