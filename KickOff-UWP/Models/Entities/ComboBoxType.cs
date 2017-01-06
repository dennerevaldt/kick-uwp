using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Entities
{
    public class ComboBoxType
    {
        public string description { get; set; }
        public string value { get; set; }

        public ComboBoxType(string description, string value)
        {
            this.description = description;
            this.value = value;
        }
    }
}
