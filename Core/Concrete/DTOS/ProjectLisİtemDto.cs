using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.DTOS
{
    public class ProjectLisItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }

        public string Content { get; set; }
        public string CoverImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryIcon { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
