using System;

namespace ChatApp.Domain
{
    public interface IEntity<Y>
         where Y : struct
    {
        Guid ID { get; set; }
        bool IsDeleted { get; set; } 
    }
    public class Entity<Y> : IEntity<Y>
        where Y : struct
    {
        public string Code { get; set; }
        public Guid ID { get; set; }
        public bool IsDeleted { get; set; } 
    }

}
