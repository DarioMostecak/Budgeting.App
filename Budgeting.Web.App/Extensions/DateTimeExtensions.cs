namespace Budgeting.Web.App.Extensions
{
    /// <summary>
    /// Represents an extension class for the DateTime structure that provides 
    /// methods for rounding time to the nearest interval.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Rounds the DateTime value to the nearest specified interval.
        /// </summary>
        /// <param name="dateTime">The DateTime value to be rounded.</param>
        /// <param name="interval">The TimeSpan representing the interval to round to.</param>
        /// <returns>The rounded DateTime value.</returns>
        public static DateTime Round(this DateTime dateTime, TimeSpan interval)
        {
            // Calculate half of the interval in ticks
            var halfIntervalTicks = (interval.Ticks + 1) >> 1;

            // Add the half interval ticks minus the remainder of the division between the total ticks and the interval ticks
            // to get the rounded DateTime value
            return dateTime.AddTicks(halfIntervalTicks - ((dateTime.Ticks + halfIntervalTicks) % interval.Ticks));
        }
    }
}
