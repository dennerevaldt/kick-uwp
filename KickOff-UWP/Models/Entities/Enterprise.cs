using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Entities
{
    public class Enterprise : Person
    {
        public string idEnterprise { get; set; }
        public string telephone { get; set; }

        public Enterprise(string idPerson, string fullName, string userName, string eMail, string password, string district, string lat, string lng, string idEnterprise, string telephone) : base(idPerson, fullName, userName, eMail, password, district, lat, lng)
        {
            this.idEnterprise = idEnterprise;
            this.telephone = telephone;
        }
    }
}
