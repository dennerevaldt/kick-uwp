using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Entities
{
    public class Court
    {
        public string id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string icon { get; set; }

        public Court() { }

        public Court(string id, string name, string category)
        {
            this.id = id;
            this.name = name;
            this.category = category;
        }
    }
}
