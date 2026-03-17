using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Concrete.DTOs
{
    public class PostListItemDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortContent { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string CoverImageUrl  { get; set; }
        public string[] Tags { get; set; }
        public DateTime PublishDate { get; set; }
    }
}
