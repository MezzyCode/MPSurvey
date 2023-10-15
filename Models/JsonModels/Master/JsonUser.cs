using Model.InfrastructurClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Model.JsonModels.Master
{
    public class JsonUser
    {
        [Required(ErrorMessage = "Username is Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please fill in Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and Confirm Password do not match.")]
        [Display(Name = "Confirm Password")]
        public string PasswordConfirm { get; set; }
        [Required(ErrorMessage = "Name is Required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string Email { get; set; }
        public string EmailApproval { get; set; }

        public string ErrorCode { get; set; }


        public string ClientID { get; set; }
        public string ID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedTime { get; set; }
        public string LastModifiedBy { get; set; }
        public Nullable<DateTime> LastModifiedTime { get; set; }

        public byte[] TimeStatus { get; set; }
        //public int RowStatus { get; }
        public int Take { get; set; }
        public int Skip { get; set; }

        private string _OrderBy;
        public string OrderBy
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
        private string _OrderByDirection;
        public string OrderByDirection
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
        public string Query { get; set; }
    }
}
