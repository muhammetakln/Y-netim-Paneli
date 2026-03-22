namespace Core.Concrete.Entities
{
    public class ProjectImage
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }   
        public int ProjectId { get; set; }     
    }
}

