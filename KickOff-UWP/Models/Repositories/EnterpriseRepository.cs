using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static async Task<dynamic> GetProximity(LatLng latLng)
        {
            string url = Constants.getBaseUrl() + "/enterprise/proximity";

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            var values = new Dictionary<string, string>();
            values.Add("lat", latLng.lat);
            values.Add("lng", latLng.lng);

            var result = await HTTP.post(url, values, token);

            try
            {
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
