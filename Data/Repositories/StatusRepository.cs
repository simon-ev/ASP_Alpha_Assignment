using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public class StatusRepository : BaseRepository<StatusEntity>, IStatusRepository
{
    public StatusRepository(AppDbContext context) : base(context)
    {
    }
}
