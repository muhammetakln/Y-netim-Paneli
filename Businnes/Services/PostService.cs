using Core.Abstracts;
using Core.Abstracts.IServices;
using Core.Concrete.DTOs;
using Core.Concrete.DTOS;
using Core.Concrete.Entities;
using Data;
using Data.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Services
{
    public class PostService : IPostService
    {
        // DÜZELTME 4: Context field olarak tutmak yerine constructor'da oluşturuluyor
        // böylece her PostService instance'ı kendi context'ini yönetir
        private readonly ApplicationDbContext context;
        private readonly IUnitOfWork unitOfWork;

        public PostService()
        {
            context = ApplicationDbContext.Create();
            unitOfWork = new UnitOfWork(context);
        }

        public void CreatePost(NewPostDto newPost)
        {
            var post = new Post
            {
                AuthorId = newPost.AuthorId,
                Content = newPost.Content,
                CoverImageUrl = newPost.CoverImageUrl,
                Title = newPost.Title,
                PublishDate = DateTime.Now,
                IsActive = false
            };
            unitOfWork.PostRepository.Create(post);
            unitOfWork.Commit();
        }

        public void DeletePost(int id, string authorId)
        {
            var post = unitOfWork.PostRepository.ReadById(id);
            if (post != null && post.AuthorId == authorId)
            {
                post.IsActive = false;
                post.IsDeleted = true;
                unitOfWork.PostRepository.Update(post);
                unitOfWork.Commit();
                // DÜZELTME 1: Silme işleminden sonra yanlışlıkla Create+Commit yapılıyordu.
                // O iki satır kaldırıldı.
            }
        }

        public PostDetailDto GetPostDetail(int id)
        {
            var post = unitOfWork.PostRepository.ReadById(id);

            if (post != null)
            {
                return new PostDetailDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    AuthorId = post.AuthorId,
                    AuthorName = $"{post.Author.FirstName} {post.Author.LastName}",
                    CoverImageUrl = post.CoverImageUrl,
                    PublishDate = post.PublishDate,
                    Tags = post.Tags.Select(x => x.Name).ToArray(),
                };
            }
            return null;
        }

        public UpDatePostDto GetPostEdit(int id)
        {
            var post = unitOfWork.PostRepository.ReadById(id);

            if (post != null)
            {
                return new UpDatePostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    AuthorId = post.AuthorId,
                    CoverImageUrl = post.CoverImageUrl,
                    IsDraft=!post.IsActive
                };
            }
            return null;
        }

      
        public void UpdatePost(UpDatePostDto updatedPost)
        {
            var post = unitOfWork.PostRepository.ReadById(updatedPost.Id);
            if (post != null)
            {
                post.Title = updatedPost.Title;
                post.Content = updatedPost.Content;
                post.CoverImageUrl = updatedPost.CoverImageUrl;
                post.PublishDate = DateTime.Now;
                post.IsActive = !updatedPost.IsDraft;
                unitOfWork.PostRepository.Update(post);
                unitOfWork.Commit(); // DÜZELTME 3: Commit eksikti, değişiklikler kaydedilmiyordu.
            }
        }

        IEnumerable<PostListItemDto> IPostService.GetPostList(string authorId)
        {
            var posts = unitOfWork.PostRepository.ReadMany(x => x.AuthorId == authorId, "Tags", "Author");
            return from post in posts
                   select new PostListItemDto
                   {
                       Id = post.Id,
                       Title = post.Title,

                   };
        }
    }
}