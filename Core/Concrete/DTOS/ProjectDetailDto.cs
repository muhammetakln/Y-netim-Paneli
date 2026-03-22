using Core.Concrete.DTOS;
using System;
using System.Collections.Generic;


namespace Core.Concrete.DTOs
{
    public class ProjectDetailDto
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Slug { get; set; }
            public string Description { get; set; }
            public string Content { get; set; }
            public string CoverImageUrl { get; set; }

            public bool IsActive { get; set; }
            public bool IsFeatured { get; set; }

            public int CategoryId { get; set; }
            public string CategoryName { get; set; }
            public string CategoryIcon { get; set; }

            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            public List<ProjectImageDto> Images { get; set; }
        }
    }

