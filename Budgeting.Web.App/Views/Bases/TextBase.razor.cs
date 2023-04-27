using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Budgeting.Web.App.Views.Bases
{
    public partial class TextBase : ComponentBase
    {
        [Parameter]
        public string Value { get; set; }

        [Parameter]
        public Align Aling { get; set; }

        [Parameter]
        public Typo Typo { get; set; }

        [Parameter]
        public string Class { get; set; }

        public void SetValue(string value)
        {
            this.Value = value;
            InvokeAsync(StateHasChanged);
        }

        public void SetAling(Align align) =>
            this.Aling = align;

        public void SetTypo(Typo typo) =>
            this.Typo = typo;

        public void SetClass(string @class) =>
           this.Class = @class;
    }
}
