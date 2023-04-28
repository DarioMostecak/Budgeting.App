using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Budgeting.Web.App.Views.Bases
{
    public partial class ContainerBase : ComponentBase
    {
        [Parameter]
        public string Style { get; set; }

        [Parameter]
        public MaxWidth MaxWidth { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

    }
}
