using Core.Abstracts.IRepositories;
using System;

namespace Core.Abstracts
{
    public interface IUnitOfWork:IDisposable
    {
        IPostRepository PostRepository { get; }
        ITagRepository TagRepository { get; }
        IProjectRepository ProjectRepository { get; }
        IProjectCategoryRepository ProjectCategoryRepository { get; }
        IProjectImageRepository ProjectImageRepository { get; }
        void Commit();
    }
    
}
