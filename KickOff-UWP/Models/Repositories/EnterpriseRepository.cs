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
    public class EnterpriseRepository
    {
        public static async Task<Enterprise> Create(Enterprise enterprise)
        {
            string url = Constants.getBaseUrl() + "/enterprise";

            var values = new Dictionary<string, string>();
            values.Add("fullname", enterprise.fullName);
            values.Add("email", enterprise.eMail);
            values.Add("username", enterprise.userName);
            values.Add("district", enterprise.district);
            values.Add("password", enterprise.password);
            values.Add("telephone", enterprise.telephone);
            values.Add("lat", enterprise.lat);
            values.Add("lng", enterprise.lng);

            dynamic result = await HTTP.post(url, values, null);

            try
            {
                if (result.id != null)
                {
                    return enterprise;
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
