using Budgeting.App.Api.Models.Users;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Budgeting.App.Api.Brokers.UserManagment
{
    public interface IUserManagerBroker
    {
        ValueTask<User> SelectUserByEmailAsync(string email);
        ValueTask<IdentityResult> InsertUserAsync(User user, string password);
        ValueTask<User> SelectUserByIdAsync(Guid userId);
        ValueTask<IdentityResult> InsertClaimsAsync(User user, IEnumerable<Claim> claims);
    }
}
