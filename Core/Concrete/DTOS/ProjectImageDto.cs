namespace Core.Concrete.DTOS
{
    public class ProjectImageDto
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public int ProjectId
        {
            get; set;
        }
    }
}
