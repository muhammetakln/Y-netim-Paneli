using Core.Abstracts.IRepositories;
using Core.Concrete.Entities;
using Data.Contexts;
using Utils.Wrapers;

namespace Data.Repositories
{
    public class ProjectCategoryRepository : Repository<ProjectCategory>, IProjectCategoryRepository
    {
        public ProjectCategoryRepository(ApplicationDbContext context) : base(context) { }
    }
}
