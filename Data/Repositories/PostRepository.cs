using Core.Abstracts.IRepositories;
using Core.Concrete.Entities;

using Data.Contexts;
using Utils.Wrapers;

namespace Data.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context) { }
    }
}
