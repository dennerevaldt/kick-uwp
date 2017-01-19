using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Entities
{
    public class Game
    {
        public string id { get; set; }
        public string name { get; set; }
        public string horary { get; set; }
        public string date { get; set; }
        public string creator_id { get; set; }
        public Schedule schedule { get; set; }
        public List<Player> playerList { get; set; }
        public Court court { get; set; }

        public Game() { }

        public Game(string id, string name, string creator_id, Schedule schedule, List<Player> listPlayer, Court court)
        {
            this.id = id;
            this.name = name;
            this.creator_id = creator_id;
            this.schedule = schedule;
            this.playerList = playerList;
            this.court = court;
        }

        public string Date
        {
            get
            {
                if (date == "" || date == null)
                {
                    return "Problemas com a data";
                }

                DateTime dt = DateTime.Parse(this.date);
                return dt.Date.ToString("dd/MM/yyyy");
            }
        }
    }
}
