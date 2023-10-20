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
        public JsonAnswer()
        {
            // Set default values in the constructor
            OrderBy = ConstantVariable.OrderByDefault;
            OrderByDirection = ConstantVariable.OrderByDirectionDefault;
        }

        [Required(ErrorMessage = "Mohon isi nama Responden")]
        public string Nama { get; set; }

        [DisplayName("Nomor KK")]
        public string? Nama_Kk { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Mohon Masukkan angka positif")]
        public int? Usia { get; set; }

        [Required(ErrorMessage = "Mohon isi alamat Responden")]
        public string Alamat { get; set; }

        public string? Rt { get; set; }

        public string? Rw { get; set; }

        public string? Kelurahan { get; set; }

        public string? Kecamatan { get; set; }

        public string? NIK { get; set; }
        [DisplayName("Nomor Telepon")]
        public string? Nomor_telp { get; set; }

        [Required(ErrorMessage = "Mohon pilih salah satu")]
        public string C1 { get; set; }

        public string? C2 { get; set; }

        [Required(ErrorMessage = "Mohon pilih salah satu")]
        public string C3A { get; set; }

        [Required(ErrorMessage = "Mohon pilih salah satu")]
        public string C3B { get; set; }

        public string C4 { get; set; }

        public string? C5 { get; set; }
        [Required(ErrorMessage = "Mohon pilih salah satu")]
        public string C6 { get; set; }

        [RequiredIfOtherSelected("C6", "Mohon isi Nama Calon")]
        public string? C6Other { get; set; }

        [Required(ErrorMessage = "Mohon pilih salah satu")]
        public string C7 { get; set; }

        public string? C8 { get; set; }

        public string? C9 { get; set; }

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

        public List<JsonHelperTable> ListKelurahan { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListKecamatan { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> List3Choice { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> List2Choice { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListCalon { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListAgama { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListPendidikan { get; set; } = new List<JsonHelperTable>();
        public List<JsonHelperTable> ListSuku { get; set; } = new List<JsonHelperTable>();
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class RequiredIfOtherSelectedAttribute : ValidationAttribute
    {
        private readonly string otherPropertyName;

        public RequiredIfOtherSelectedAttribute(string otherPropertyName, string errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
            this.ErrorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(otherPropertyName);
            var otherValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (otherValue != null && otherValue.ToString() == "Other")
            {
                if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }

            return ValidationResult.Success;
        }
    }
}
