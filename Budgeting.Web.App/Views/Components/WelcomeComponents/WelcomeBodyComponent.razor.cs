using Budgeting.Web.App.Views.Bases;
using Microsoft.AspNetCore.Components;

namespace Budgeting.Web.App.Views.Components.WelcomeComponents
{
    public partial class WelcomeBodyComponent : ComponentBase
    {
        public TextBase? TitleText { get; set; }
        public TextBase? ParagraphText { get; set; }
        public TextBase? ImageCardText { get; set; }
        public CardSliderBase? ImageSlider { get; set; }

    }
}
