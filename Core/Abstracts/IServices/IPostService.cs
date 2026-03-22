using Core.Concrete.DTOs;
using Core.Concrete.DTOS;
using System.Collections.Generic;

namespace Core.Abstracts.IServices
{
    public interface IPostService
    {
        IEnumerable<PostListItemDto> GetPostList(string authorId);
        PostDetailDto GetPostDetail(int id);
        UpDatePostDto GetPostEdit(int id);
        void CreatePost(NewPostDto newPostDto);
        void UpdatePost(UpDatePostDto upDatePostDto);
        void DeletePost(int id,string authorId);
    }
}
