using Core.Abstracts.IRepositories;
using Core.Concrete.Entities;
using Data.Contexts;

using Utils.Wrapers;

namespace Data.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context) { }
    }
}
