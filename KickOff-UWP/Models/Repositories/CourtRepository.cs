using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Repositories
{
    public class CourtRepository
    {
        public static async Task<Court> Create(Court court)
        {
            string url = Constants.getBaseUrl() + "/court";

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            var values = new Dictionary<string, string>();
            values.Add("name", court.name);
            values.Add("category", court.category);

            dynamic result = await HTTP.post(url, values, token);

            try
            {
                if (result.id != null)
                {
                    return court;
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

        public static async Task<ObservableCollection<Court>> GetAll()
        {
            string url = Constants.getBaseUrl() + "/court";

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            dynamic result = await HTTP.get(url, token);

            ObservableCollection<Court> list = JsonConvert.DeserializeObject<ObservableCollection<Court>>(result);

            foreach (Court item in list)
            {
                if (item.category == "Futebol society (7)")
                {
                    item.icon = "/Assets/icon_society.png";
                }

                if (item.category == "Futebol de salão (Futsal)")
                {
                    item.icon = "/Assets/icon_futsal.png";
                }
            }

            return list;
        }

        public static async Task<Court> Delete(Court court)
        {
            string url = Constants.getBaseUrl() + "/court/" + court.id;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            dynamic result = await HTTP.delete(url, token);

            try
            {
                if (result.message != null)
                {
                    return court;
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

        public static async Task<Court> Update(Court court)
        {
            string url = Constants.getBaseUrl() + "/court/" + court.id;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            var values = new Dictionary<string, string>();
            values.Add("name", court.name);
            values.Add("category", court.category);

            dynamic result = await HTTP.put(url, values, token);

            try
            {
                if (result.id != null)
                {
                    return court;
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
