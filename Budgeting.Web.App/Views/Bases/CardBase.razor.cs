using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Budgeting.Web.App.Views.Bases
{
    public partial class CardBase
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public string Style { get; set; }

        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public Align AlignTitle { get; set; }

        [Parameter]
        public RenderFragment CardContent { get; set; }

        [Parameter]
        public RenderFragment CardAction { get; set; }
    }
}
