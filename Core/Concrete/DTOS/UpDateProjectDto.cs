using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Core.Concrete.DTOs
{
    public class UpdateProjectDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public string Slug { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Content is required")]
        [AllowHtml]
        public string Content { get; set; }

        public string CoverImageUrl { get; set; }

        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
    }
}