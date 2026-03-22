using Core.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.Entities
{
    public class Project : BaseEntity
    {

        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }

        public string CoverImageUrl { get; set; }



        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }



        public int CategoryId { get; set; }
        public virtual ProjectCategory Category { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}

