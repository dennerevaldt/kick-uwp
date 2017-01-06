using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Repositories
{
    public class PlayerRepository
    {
        public static async Task<Player> Create(Player player)
        {
            string url = Constants.getBaseUrl() + "/player";

            var values = new Dictionary<string, string>();
            values.Add("fullname", player.fullName);
            values.Add("email", player.eMail);
            values.Add("username", player.userName);
            values.Add("district", player.district);
            values.Add("password", player.password);
            values.Add("position", player.position);
            values.Add("lat", player.lat);
            values.Add("lng", player.lng);

            dynamic result = await HTTP.post(url, values, null);

            try
            {
                if (result == HttpStatusCode.OK)
                {
                    return player;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
