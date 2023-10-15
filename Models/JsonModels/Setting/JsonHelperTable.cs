using Microsoft.AspNetCore.Http;
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
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<DateTime> LastModifiedTime { get; set; }

        public byte[] TimeStatus { get; set; }
        public int RowStatus { get; }
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string ClientID { get; set; }
    }
}
