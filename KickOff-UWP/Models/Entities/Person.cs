using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Entities
{
    public abstract class Person
    {
        public string idPerson { get; set; }
        public string fullName { get; set; }
        public string userName { get; set; }
        public string eMail { get; set; }
        public string password { get; set; }
        public string district { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }

        public Person(string idPerson, string fullName, string userName, string eMail, string password, string district, string lat, string lng)
        {
            this.idPerson = idPerson;
            this.fullName = fullName;
            this.userName = userName;
            this.eMail = eMail;
            this.password = password;
            this.district = district;
            this.lat = lat;
            this.lng = lng;
        }
    }
}
