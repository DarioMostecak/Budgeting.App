using MudBlazor;

namespace Budgeting.Web.App.Brokers.Toasts
{
    public interface IToastBroker
    {
        public void AddToast(string message, Severity severity);
    }
}
