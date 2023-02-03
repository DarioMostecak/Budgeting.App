namespace Budgeting.App.Api.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        public DateTime GetCurrentDateTime() =>
            DateTime.UtcNow;
    }
}
