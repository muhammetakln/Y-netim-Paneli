using Core.Abstracts.IRepositories;
using Core.Concrete.Entities;
using Data.Contexts;
using Utils.Wrapers;

namespace Data.Repositories
{
    public class ProjectImageRepository : Repository<ProjectImage>, IProjectImageRepository
    {
        public ProjectImageRepository(ApplicationDbContext context) : base(context) { }
    }
 
}
