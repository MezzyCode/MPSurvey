using System;
using System.Collections.Generic;
using System.Text;
using static Database.Context.HelperFunction;

namespace Database.Context
{
    public interface IEntity
    {
        string ID { get; }
        string CreatedBy { get; set; }
        DateTime CreatedTime { get; set; }
        string LastModifiedBy { get; set; }
        Nullable<DateTime> LastModifiedTime { get; set; }

        byte[] TimeStatus { get; set; }
        int RowStatus { get; }
        ObjectState ModelState { get; set; }
        bool IsTrackedId { get; set; }

        public string ClientID { get; set; }



    }

    public interface IEntity<T> : IEntity
    {
        new T ID { get; set; }
    }
}
