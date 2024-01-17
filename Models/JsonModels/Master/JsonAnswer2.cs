using Microsoft.AspNetCore.Http;
using Model.InfrastructurClass;
using Model.JsonModels.Setting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.JsonModels.Master
{
    public class IndexAnswer2VM
    {
        public List<JsonAnswer2> listIndex { get; set; }
        public IFormFile Upload { get; set; }
        public string UrlFileLog { get; set; }
    }
    public class JsonAnswer2
    {
        public JsonAnswer2()
        {
            // Set default values in the constructor
            OrderBy = ConstantVariable.OrderByDefault;
            OrderByDirection = ConstantVariable.OrderByDirectionDefault;
        }

        [Required(ErrorMessage = "Mohon isi Nama Responden")]
        public string? Nama { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Mohon Masukkan angka positif")]
        public int? Usia { get; set; }

        [Required(ErrorMessage = "Mohon isi alamat Responden")]
        public string Alamat { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Mohon Masukkan angka positif")]
        public string? Rt { get; set; }
        [Required(ErrorMessage = "Mohon isi RW")]
        [Range(1, int.MaxValue, ErrorMessage = "Mohon Masukkan angka positif")]
        public string? Rw { get; set; }

        [Required(ErrorMessage = "Mohon isi Kelurahan")]
        public string? Kelurahan { get; set; }

        [Required(ErrorMessage = "Mohon isi Kecamatan")]
        public string? Kecamatan { get; set; }

        public string? NIK { get; set; }
        [Required(ErrorMessage = "Mohon isi No. Telpon")]
        public string? NoTelp { get; set; }
        [Required(ErrorMessage = "Mohon isi Nama Simpul")]
        public string? Simpul { get; set; }

        [Required(ErrorMessage = "Mohon pilih salah satu")]
        public string C1 { get; set; }

        [Required(ErrorMessage = "Mohon pilih salah satu")]
        public string? C2 { get; set; }

        [Required(ErrorMessage = "Mohon pilih salah satu")]
        public string C3 { get; set; }

        [Required(ErrorMessage = "Mohon pilih salah satu")]
        public string C4 { get; set; }
        public string? Kota { get; set; }


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

        public List<JsonHelperTable> ListRw { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListKelurahan { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListKecamatan { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> List3Choice { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> List2Choice { get; set; } = new List<JsonHelperTable>();
    }
}
