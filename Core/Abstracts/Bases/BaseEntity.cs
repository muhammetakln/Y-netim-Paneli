using System;

namespace Core.Abstracts
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }= DateTime.Now;
        public bool Active { get; set; } = true;
        public bool Deleted { get; set; } = false;
    }
}
