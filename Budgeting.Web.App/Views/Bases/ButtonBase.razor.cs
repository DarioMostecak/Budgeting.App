using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Budgeting.Web.App.Views.Bases
{
    public partial class ButtonBase : ComponentBase
    {
        [Parameter]
        public ButtonType ButtonType { get; set; }

        [Parameter]
        public string Label { get; set; }

        [Parameter]
        public Color Color { get; set; }

        [Parameter]
        public Action OnClick { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; }

        [Parameter]
        public Variant Variant { get; set; }

        [Parameter]
        public string Class { get; set; }

        public void Click() => OnClick.Invoke();

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
