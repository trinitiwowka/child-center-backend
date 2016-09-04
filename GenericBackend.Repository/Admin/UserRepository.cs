using System.Collections.Generic;
using System.Linq;
using GenericBackend.DataModels;

namespace GenericBackend.Repository.Admin
{
    public class UserRepository : MongoRepository<IdentityUser>, IMongoRepository<IdentityUser>
    {
        public IEnumerable<IdentityUser> EndUsers()
        {
            return this.Where(x => x.Roles.Contains("User")).ToArray();
        }
    }
}
