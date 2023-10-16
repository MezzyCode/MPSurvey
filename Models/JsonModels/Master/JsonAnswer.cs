using Model.InfrastructurClass;
using Model.JsonModels.Setting;
using Model.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JsonModels.Master
{
    public class IndexAnswerVM
    {
        public List<JsonAnswer> listIndex { get; set; }
    }

    public class JsonAnswer
    {
        public string Nama { get; set; }

        [DisplayName("Nomor KK")]
        public string? Nama_Kk { get; set; }

        public int? Usia { get; set; }

        public string Alamat { get; set; }

        public string Rt { get; set; }

        public string Rw { get; set; }

        public string Kelurahan { get; set; }

        public string Kecamatan { get; set; }

        public string? NIK { get; set; }
        [DisplayName("Nomor Telepon")]
        public string? Nomor_telp { get; set; }

        public string C1 { get; set; }

        public string? C2 { get; set; }

        public string C3A { get; set; }

        public string C3B { get; set; }

        public string C4 { get; set; }

        public string? C5 { get; set; }

        public string C6 { get; set; }

        public string C7 { get; set; }

        public string C8 { get; set; }

        public string C9 { get; set; }

        public string? C10 { get; set; }

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
        public string? OrderBy
        {
            get
            {
                return _OrderBy;
            }
            set
            {
                _OrderBy = value;
                if (!String.IsNullOrEmpty(_OrderBy))
                {
                    _OrderBy = value;
                }
                else
                {
                    _OrderBy = ConstantVariable.OrderByDefault;
                }
            }
        }
        private string? _OrderByDirection;
        public string? OrderByDirection
        {
            get
            {
                return _OrderByDirection;
            }
            set
            {
                _OrderByDirection = value;
                if (!String.IsNullOrEmpty(_OrderBy))
                {
                    _OrderByDirection = value;
                }
                else
                {
                    _OrderByDirection = ConstantVariable.OrderByDirectionDefault;
                }
            }
        }
        public string? Query { get; set; }

        public List<JsonHelperTable> ListKelurahan { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListKecamatan { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> List3Choice { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> List2Choice { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListCalon { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListAgama { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListPendidikan { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListSuku { get; set; } = new List<JsonHelperTable>();
    }
}
