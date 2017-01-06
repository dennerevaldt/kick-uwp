using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Entities
{
    public class LatLng
    {
        public string lat { get; set; }
        public string lng { get; set; }

        public LatLng () { }

        public LatLng (string lat, string lng)
        {
            this.lat = lat;
            this.lng = lng;
        }
    }
}
