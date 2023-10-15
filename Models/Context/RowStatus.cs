using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Context
{
    public enum RowStatus : int
    {
        Deleted = -1,
        Active = 0,
        InActive = 1
    }
}
