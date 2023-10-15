using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Context
{
    public static class HelperFunction
    {
        public static EntityState ConvertState(ObjectState state)
        {
            switch (state)
            {
                case ObjectState.Added:
                    return EntityState.Added;
                case ObjectState.Modified:
                    return EntityState.Modified;
                case ObjectState.SoftDelete:
                    return EntityState.Modified;
                case ObjectState.Unchanged:
                    return EntityState.Unchanged;
                case ObjectState.HardDelete:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }
        public enum ObjectState : int
        {
            Unchanged = 0,
            Added = 1,
            Modified = 2,
            SoftDelete = 3,
            HardDelete = 98,
            Processed = 99
        }
    }
}