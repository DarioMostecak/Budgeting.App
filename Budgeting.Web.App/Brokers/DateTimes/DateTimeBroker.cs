using Budgeting.Web.App.Extensions;

namespace Budgeting.Web.App.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        public DateTime GetCurrentDateTime() =>
            DateTime.UtcNow.Round(new TimeSpan(0, 0, 0, 1));
    }
}
