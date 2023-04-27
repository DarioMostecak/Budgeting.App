using MudBlazor;

namespace Budgeting.Web.App.Brokers.Toasts
{
    public class ToastBroker : IToastBroker
    {
        public readonly ISnackbar snackbar;

        public ToastBroker(ISnackbar snackbar)
        {
            this.snackbar = snackbar;
        }

        public void AddToast(string message, Severity severity) =>
            this.snackbar.Add(message, severity);
    }
}
