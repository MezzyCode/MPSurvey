using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JsonModels.Setting
{
    public class IndexFileLogVM
    {
        public List<JsonFileLog> listIndex { get; set; }
        public IFormFile Upload { get; set; }
        public string UrlFileLog { get; set; }
    }

    public class JsonFileLog
    {
        public string ID { get; set; }
        public string TableName { get; set; }
        public string FileName { get; set; }
        public bool? Status { get; set; }
        public string Remarks { get; set; }
        public string ClientID { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedByName { get; set; }
        public string CreatedByEmployeeID { get; set; }
        public string CreatedByEmployeeNIK { get; set; }
        public string LastModifiedByEmployeeID { get; set; }
        public string LastModifiedByEmployeeNIK { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public byte[] TimeStatus { get; set; }
        public int RowStatus { get; set; }

        public int Take { get; set; }
        public int Skip { get; set; }

        private string _OrderBy;

        public string OrderBy
        {
            get
            {
                if (!String.IsNullOrEmpty(_OrderBy))
                {
                    return _OrderBy;
                }
                else
                {
                    _OrderBy = InfrastructurClass.ConstantVariable.OrderByDefault;
                }
                return _OrderBy;
            }
            set
            {
                _OrderBy = value;
            }
        }
        private string _OrderByDirection;
        public string OrderByDirection
        {
            get
            {
                if (!string.IsNullOrEmpty(_OrderByDirection))
                {
                    return _OrderByDirection;
                }
                else
                {
                    _OrderByDirection = InfrastructurClass.ConstantVariable.OrderByDirectionDefault;
                }
                return _OrderByDirection;
            }
            set
            {
                _OrderByDirection = value;
            }
        }

        public string Query { get; set; }
    }
    public partial class JsonFileLogDetail
    {
        public string ID { get; set; }
        public string IDData { get; set; }
        public string IDFileLog { get; set; }
        public int? OrderNo { get; set; }
        public bool? Status { get; set; }
        public string Remarks { get; set; }
        public string SourceTxt { get; set; }
        public string CodeData { get; set; }
        public string ClientID { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedBy { get; set; }
        public string LastModifiedByName { get; set; }
        public string CreatedByEmployeeID { get; set; }
        public string CreatedByEmployeeNIK { get; set; }
        public string LastModifiedByEmployeeID { get; set; }
        public string LastModifiedByEmployeeNIK { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedTime { get; set; }
        public byte[] TimeStatus { get; set; }
        public int RowStatus { get; set; }

        public int Take { get; set; }
        public int Skip { get; set; }

        private string _OrderBy;

        public string OrderBy
        {
            get
            {
                if (!String.IsNullOrEmpty(_OrderBy))
                {
                    return _OrderBy;
                }
                else
                {
                    _OrderBy = InfrastructurClass.ConstantVariable.OrderByDefault;
                }
                return _OrderBy;
            }
            set
            {
                _OrderBy = value;
            }
        }
        private string _OrderByDirection;
        public string OrderByDirection
        {
            get
            {
                if (!string.IsNullOrEmpty(_OrderByDirection))
                {
                    return _OrderByDirection;
                }
                else
                {
                    _OrderByDirection = InfrastructurClass.ConstantVariable.OrderByDirectionDefault;
                }
                return _OrderByDirection;
            }
            set
            {
                _OrderByDirection = value;
            }
        }

        public string Query { get; set; }
    }
}
