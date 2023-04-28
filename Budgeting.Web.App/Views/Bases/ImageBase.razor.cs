using Microsoft.AspNetCore.Components;

namespace Budgeting.Web.App.Views.Bases
{
    public partial class ImageBase
    {
        [Parameter]
        public string Src { get; set; }

        [Parameter]
        public string Width { get; set; }

        [Parameter]
        public string Height { get; set; }

        [Parameter]
        public string Class { get; set; }
    }
}
