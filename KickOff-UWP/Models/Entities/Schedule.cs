using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Entities
{
    public class Schedule
    {
        public string id { get; set; }
        public string date { get; set; }
        public string horary { get; set; }
        public Court court { get; set; }

        public Schedule() { }

        public Schedule(string id, string date, string horary, Court court)
        {
            this.id = id;
            this.date = date;
            this.horary = horary;
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
