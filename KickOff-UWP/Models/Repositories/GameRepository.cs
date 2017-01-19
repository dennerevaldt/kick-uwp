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
    class GameRepository
    {
        public static async Task<Game> Create(Game game)
        {
            string url = Constants.getBaseUrl() + "/game";

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            var values = new Dictionary<string, string>();
            values.Add("name", game.name);
            values.Add("schedule_id", game.schedule.id);

            dynamic result = await HTTP.post(url, values, token);

            try
            {
                if (result.id != null)
                {
                    return game;
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

        public static async Task<ObservableCollection<Game>> GetAll()
        {
            string url = Constants.getBaseUrl() + "/schedule";

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            dynamic result = await HTTP.get(url, token);

            ObservableCollection<Game> list = JsonConvert.DeserializeObject<ObservableCollection<Game>>(result);

            foreach (Game item in list)
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

        public static async Task<Game> Delete(Game game)
        {
            string url = Constants.getBaseUrl() + "/game/" + game.id;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            dynamic result = await HTTP.delete(url, token);

            try
            {
                if (result.message != null)
                {
                    return game;
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
