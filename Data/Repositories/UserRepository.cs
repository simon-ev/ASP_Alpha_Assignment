using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}
