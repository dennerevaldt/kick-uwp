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
                if (result[0][0]["game_id"] != null)
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
            string url = Constants.getBaseUrl() + "/game";

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;

            string token = AuthRepository.getCredentials(user);

            dynamic result = await HTTP.get(url, token);
            dynamic listDes = JsonConvert.DeserializeObject<List<dynamic>>(result);
            ObservableCollection<Game> list = new ObservableCollection<Game>();

            foreach (var item in listDes)
            {
                list.Add(new Game(
                    item["id"].ToString(), 
                    item["name"].ToString(), 
                    item["creator_id"].ToString(), 
                    new Schedule(item["Schedule"]["id"].ToString(), item["Schedule"]["date"].ToString(), item["Schedule"]["horary"].ToString(), null), 
                    null, 
                    new Court(item["Schedule"]["Court"]["id"].ToString(), item["Schedule"]["Court"]["name"].ToString(), item["Schedule"]["Court"]["category"].ToString()))
                );
            }

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
