using Budgeting.App.Api.Extensions;

namespace Budgeting.App.Api.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        public DateTime GetCurrentDateTime() =>
            DateTime.UtcNow.Round(new TimeSpan(0, 0, 0, 1));
    }
}
