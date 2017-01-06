using KickOff_UWP.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Utils
{
    public class PlaceAPI
    {
        private static string PLACES_API_BASE = "https://maps.googleapis.com/maps/api/place";
        private static string TYPE_DETAILS = "/details";
        private static string TYPE_AUTOCOMPLETE = "/autocomplete";
        private static string OUT_JSON = "/json";
        private static string API_KEY = "AIzaSyD6bzbwKaZKiHy12YpFQr7D-B4ETA5noKo";

        public static async Task<List<Place>> autocomplete(string place)
        {
            StringBuilder jsonResults = new StringBuilder();

            StringBuilder sb = new StringBuilder(PLACES_API_BASE + TYPE_AUTOCOMPLETE + OUT_JSON);
            sb.Append("?key=" + API_KEY);
            sb.Append("&types=(cities)");
            sb.Append("&input=" + place);
            sb.Append("&language=pt_BR");
            sb.Append("&components=country:br");

            //Create an HTTP client object
            HttpClient httpClient = new HttpClient();

            Uri url = new Uri(sb.ToString());

            //Send the GET request asynchronously and retrieve the response as a string.
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";
            List<Place> places = new List<Place>();

            try
            {     
                httpResponse = await httpClient.GetAsync(url);
                //Send the GET request
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

                dynamic results = JsonConvert.DeserializeObject<dynamic>(httpResponseBody);

                foreach (var item in results.predictions)
                {
                    string id = (string) item.place_id;
                    string desc = (string) item.description;
                    places.Add(new Place(id, desc));
                }

            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                return null;
            }

            return places;
        }

        public static async Task<Place> latLng(Place place)
        {
            StringBuilder jsonResults = new StringBuilder();

            StringBuilder sb = new StringBuilder(PLACES_API_BASE + TYPE_DETAILS + OUT_JSON);
            sb.Append("?key=" + API_KEY);
            sb.Append("&placeid=" + place.idPlace);

            //Create an HTTP client object
            HttpClient httpClient = new HttpClient();

            Uri url = new Uri(sb.ToString());

            //Send the GET request asynchronously and retrieve the response as a string.
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                httpResponse = await httpClient.GetAsync(url);
                //Send the GET request
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

                dynamic data = JsonConvert.DeserializeObject<dynamic>(httpResponseBody);

                place.latLng = new LatLng();
                string lat = (string) data.result.geometry.location.lat;
                string lng = (string) data.result.geometry.location.lng;
                place.latLng.lat = lat;
                place.latLng.lng = lng;
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                return null;
            }

            return place;
        }
    }
}
