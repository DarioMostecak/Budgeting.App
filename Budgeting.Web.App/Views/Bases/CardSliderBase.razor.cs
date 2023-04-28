using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Budgeting.Web.App.Views.Bases
{
    public partial class CardSliderBase : ComponentBase
    {
        [Parameter]
        public string Class { get; set; }

        [Parameter]
        public string Style { get; set; }

        [Parameter]
        public bool ShowArrows { get; set; }

        [Parameter]
        public bool ShowBullets { get; set; }

        [Parameter]
        public bool AutoCycle { get; set; }

        [Parameter]
        public Align AlignTitle { get; set; }

        [Parameter]
        public Transition TransitionType { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

    }
}
