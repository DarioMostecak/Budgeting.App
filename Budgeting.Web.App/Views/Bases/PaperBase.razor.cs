using Microsoft.AspNetCore.Components;

namespace Budgeting.Web.App.Views.Bases
{
    public partial class PaperBase : ComponentBase
    {
        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public string MaxWidth { get; set; }

        [Parameter]
        public string MaxHeight { get; set; }

        [Parameter]
        public string Style { get; set; }

        [Parameter]
        public RenderFragment PaperContent { get; set; }
    }
}
