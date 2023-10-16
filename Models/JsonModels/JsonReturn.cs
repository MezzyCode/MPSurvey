using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Model.JsonModels
{
    public class JsonReturn
    {
        [Required]
        public bool result { get; set; }
        [Required]
        public string message { get; set; }
        public object ObjectValue { get; set; }

        public JsonReturn(Boolean? flag)
        {
            if (flag == null)
            {
                result = true;
            }
            else
            {
                result = Convert.ToBoolean(flag);
            }
        }

    }
}
