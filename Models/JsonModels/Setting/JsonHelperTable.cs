using Microsoft.AspNetCore.Http;
using Model.InfrastructurClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JsonModels.Setting
{
    public class IndexHelperTableVM
    {
        public List<JsonHelperTable> listIndex { get; set; }
        public IFormFile Upload { get; set; }
        public string UrlFileLog { get; set; }
    }
    public class JsonHelperTable
    {
        public JsonHelperTable()
        {
            // Set default values in the constructor
            OrderBy = ConstantVariable.OrderByDefault;
            OrderByDirection = ConstantVariable.OrderByDirectionDefault;
        }

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
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
