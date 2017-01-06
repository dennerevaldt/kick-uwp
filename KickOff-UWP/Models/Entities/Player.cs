using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Entities
{
    public class Player : Person
    {
        public string idPlayer { get; set; }
        public string position { get; set; }

        public Player(string idPerson, string fullName, string userName, string eMail, string password, string district, string lat, string lng, string idPlayer, string position) : base(idPerson, fullName, userName, eMail, password, district, lat, lng)
        {
            this.idPlayer = idPlayer;
            this.position = position;
        }
    }
}
