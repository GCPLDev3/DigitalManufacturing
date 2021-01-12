using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFG_DigitalApp.Models
{
    public class ParameterModel
    {
        public string ParameterName { get; set; }
        public int ParameterId { get; set; }
        public int ParameterTypeId { get; set; }
        public string ParameterTypeName { get; set; }
        public string ToolRequired { get; set; }
    }
}