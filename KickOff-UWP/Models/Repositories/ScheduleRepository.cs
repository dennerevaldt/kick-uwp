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
    class ScheduleRepository
    {
        public static async Task<Schedule> Create(Schedule schedule)
        {
            string url = Constants.getBaseUrl() + "/schedule";

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            var values = new Dictionary<string, string>();
            values.Add("horary", schedule.horary);
            values.Add("date", schedule.date);
            values.Add("court_id", schedule.court.id);

            dynamic result = await HTTP.post(url, values, token);

            try
            {
                if (result.id != null)
                {
                    return schedule;
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

        public static async Task<ObservableCollection<Schedule>> GetAll()
        {
            string url = Constants.getBaseUrl() + "/schedule";

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            dynamic result = await HTTP.get(url, token);

            ObservableCollection<Schedule> list = JsonConvert.DeserializeObject<ObservableCollection<Schedule>>(result);

            foreach (Schedule item in list)
            {
                if (item.court.category == "Futebol society (7)")
                {
                    item.court.icon = "/Assets/icon_society.png";
                }

                if (item.court.category == "Futebol de salão (Futsal)")
                {
                    item.court.icon = "/Assets/icon_futsal.png";
                }
            }

            return list;
        }

        public static async Task<Schedule> Delete(Schedule schedule)
        {
            string url = Constants.getBaseUrl() + "/schedule/" + schedule.id;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            dynamic result = await HTTP.delete(url, token);

            try
            {
                if (result.message != null)
                {
                    return schedule;
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

        public static async Task<Schedule> Update(Schedule schedule)
        {
            string url = Constants.getBaseUrl() + "/schedule/" + schedule.id;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            var values = new Dictionary<string, string>();
            values.Add("horary", schedule.horary);
            values.Add("date", schedule.date);
            values.Add("court_id", schedule.court.id);

            dynamic result = await HTTP.put(url, values, token);

            try
            {
                if (result.id != null)
                {
                    return schedule;
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
