using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Budgeting.Web.App.Views.Bases
{
    public partial class CardSliderItemBase : ComponentBase
    {
        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public Transition TransitionType { get; set; }

        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public Align AlignTitle { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
    }
}
