
namespace Core.Concrete.DTOs
{
    public class NewPostDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorId { get; set; }
        public string CoverImageUrl { get; set; }
        public string[] Tags { get; set; }
    }
}

