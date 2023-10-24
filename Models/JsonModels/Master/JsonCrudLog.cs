using Model.InfrastructurClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JsonModels.Master
{
    public class JsonCrudLog
    {
        public string? TableName { get; set; }
        public string? ProcessName { get; set; }
        public string? Data { get; set; }
        public bool? Status { get; set; }
        public string? Remarks { get; set; }

        public string? ID { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedTime { get; set; }
        public string? LastModifiedBy { get; set; }
        public Nullable<DateTime> LastModifiedTime { get; set; }

        public byte[]? TimeStatus { get; set; }
        public string? ClientID { get; set; }
        public int? RowStatus { get; }
        public int? Take { get; set; }
        public int? Skip { get; set; }

        private string? _OrderBy;
        public string OrderBy
        {
            get
            {
                return _OrderBy;
            }
            set
            {
                _OrderBy = !string.IsNullOrEmpty(value) ? value : ConstantVariable.OrderByDefault;
            }
        }
        private string? _OrderByDirection;
        public string OrderByDirection
        {
            get
            {
                return _OrderByDirection;
            }
            set
            {
                _OrderByDirection = !string.IsNullOrEmpty(value) ? value : ConstantVariable.OrderByDirectionDefault;
            }
        }
        public string? Query { get; set; }
    }
}
