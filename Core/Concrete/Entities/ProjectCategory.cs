using Core.Abstracts;

namespace Core.Concrete.Entities
{
    public class ProjectCategory : BaseEntity
    {
                  
        public string Name { get; set; }         
         
        public string Description { get; set; } 
        public string Icon { get; set; }         
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}

