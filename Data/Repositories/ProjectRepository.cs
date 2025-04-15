using Data.Contexts;
using Data.Entities;

namespace Data.Repositories;

public class ProjectRepository : BaseRepository<ProjectEntity>, IProjectRepository    
{
    public ProjectRepository(AppDbContext context) : base(context)
    {
    }
}
