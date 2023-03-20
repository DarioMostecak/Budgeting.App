using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Budgeting.App.Api.Models.Users
{
    [CollectionName("users")]
    public sealed class User : MongoIdentityUser<Guid>
    {
        public override string UserName
        {
            get => base.Email;
            set => base.Email = value;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
