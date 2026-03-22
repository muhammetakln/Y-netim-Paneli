using System.ComponentModel.DataAnnotations;


namespace Core.Concrete.DTOs
{
    public class NewProjectDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; }

        public string Slug { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Content is required")]
       
        public string Content { get; set; }

        // ✅ Opsiyonel - Required YOK
        [Display(Name = "Cover Image")]
        public string CoverImageUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
    }
    }

