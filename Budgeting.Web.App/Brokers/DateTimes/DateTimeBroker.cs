namespace Budgeting.Web.App.Brokers.DateTimes
{
    public class DateTimeBroker : IDateTimeBroker
    {
        public DateTime GetCurrentDateTime() =>
            DateTime.UtcNow;
    }
}
