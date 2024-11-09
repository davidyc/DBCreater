using DBCreater.Models;
using DBCreater.Repositories.Interfaces;
using DBCreater.SQL;

namespace DBCreater.Repositories.Implementation
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TestDBContext context) : base(context)
        {
        }
    }
}
