using System;
using System.Collections.Generic;

namespace Model.Models
{
    public partial class CrudLog
    {
        public string? TableName { get; set; }
        public string? ProcessName { get; set; }
        public string? Data { get; set; }
        public bool? Status { get; set; }
        public string? Remarks { get; set; }
    }
}
