using Core.Abstracts.IRepositories;
using Core.Concrete.Entities;
using Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Wrapers;

namespace Data.Repositories
{
    public class ProjectRepository:Repository<Project>,IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context) { }
    }
 
}
