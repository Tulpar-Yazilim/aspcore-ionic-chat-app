using System;

namespace ChatApp.Domain
{
    public interface ITableEntity<Y> : IEntity<Y>
        where Y : struct
    {
        DateTime CreateDT { get; set; }
        DateTime? UpdateDT { get; set; }
        Guid? CreateByID { get; set; }
        User CreateBy { get; set; }
        Guid? UpdateByID { get; set; }
        User UpdateBy { get; set; } 
    }

    public class TableEntity<Y> : Entity<Y>, ITableEntity<Y>
        where Y : struct
    { 
        public DateTime CreateDT { get; set; }
        public DateTime? UpdateDT { get; set; }
        public Guid? CreateByID { get; set; }
        public virtual User CreateBy { get; set; }
        public Guid? UpdateByID { get; set; }
        public virtual User UpdateBy { get; set; } 
    }


    public class UserTableEntity<Y> : Entity<Y>
       where Y : struct
    {
        public DateTime CreateDT { get; set; }
        public DateTime? UpdateDT { get; set; }
    }
     
}
