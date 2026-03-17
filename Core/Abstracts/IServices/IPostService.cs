using Core.Concrete.DTOs;
using System.Collections.Generic;

namespace Core.Abstracts.IServices
{
    public interface IPostService
    {
        IEnumerable<PostListItemDto> GetPostList();
        PostDetailDto GetPostDetail(int id);

        void CreatePost(NewPostDto newPostDto);
        void UpdatePost(UpDatePostDto UpDatePostDto);
        void DeletePost(int id,string authorId);
    }
}
