using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public class ClientRepository : BaseRepository<ClientEntity>, IClientRepository
{
    public ClientRepository(AppDbContext context) : base(context)
    {
    }
}
