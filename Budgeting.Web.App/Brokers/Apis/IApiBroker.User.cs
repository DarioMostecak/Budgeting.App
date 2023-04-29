using Budgeting.Web.App.Models.Users;

namespace Budgeting.Web.App.Brokers.Apis
{
    public partial interface IApiBroker
    {
        ValueTask<User> PostUserAsync(User user, string password);
    }
}
