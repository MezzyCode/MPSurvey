using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Database.Context.HelperFunction;

namespace Database.Context
{
    public class EntityBase
    {
        [NotMapped]
        public ObjectState ModelState
        {
            get;
            set;
        }

        [NotMapped]
        public bool IsTrackedId { get; set; }
    }

    public abstract class Entity<T> : EntityBase, IEntity<string>
    {
        [Key]
        public string ID { get; set; }
        string IEntity.ID
        {
            get { return Guid.NewGuid().ToString("N").ToUpper(); }
        }

        [StringLength(300)]
        public virtual string CreatedBy { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        [StringLength(300)]
        public virtual string LastModifiedBy { get; set; }
        public virtual Nullable<DateTime> LastModifiedTime { get; set; }
        [Timestamp, ConcurrencyCheck]
        public virtual byte[] TimeStatus { get; set; }
        public virtual int RowStatus { get; private set; }
        public string ClientID { get; set; }
        public virtual void SetRowStatus(RowStatus value)
        {
            RowStatus = (int)value;
        }

    }

}