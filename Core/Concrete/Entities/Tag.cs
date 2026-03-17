using Core.Abstracts;


namespace Core.Concrete.Entities
{
    public class Tag : BaseEntity
    {
        public string Name { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }

    }

}
