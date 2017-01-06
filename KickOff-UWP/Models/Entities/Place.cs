using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Entities
{
    public class Place
    {
        public string idPlace { get; set; }
        public string description { get; set; }
        public LatLng latLng { get; set; }

        public Place() { }

        public Place(string idPlace, string description)
        {
            this.idPlace = idPlace;
            this.description = description;
        }
    }
}
