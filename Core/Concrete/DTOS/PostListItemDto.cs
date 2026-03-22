using System;

namespace Core.Concrete.DTOs
{
    public class PostListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string CoverImageUrl { get; set; }

        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIcon { get; set; }

        public DateTime CreatedAt { get; set; }
        public string ShortContent { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime PublishDate { get; set; }
        public string[] Tags { get; set; }
    }
}