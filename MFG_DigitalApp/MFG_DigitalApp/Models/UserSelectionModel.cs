using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MFG_DigitalApp.Models
{
    public class UserSelectionModel
    {
        public string Username { get; set; }
        public string PlantCode { get; set; }
        public string Line { get; set; }
        public string ShiftCode { get; set; }
        public string ShiftDate { get; set; }
    }
}