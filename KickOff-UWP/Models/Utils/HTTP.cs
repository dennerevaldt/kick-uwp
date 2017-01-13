using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Utils
{
    public class HTTP
    {
        public static async Task<dynamic> get(string url, string token)
        {
            HttpClient httpClient = new HttpClient();

            Uri requestUri = new Uri(url);

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Add("x-access-token", token);
            }

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                return httpResponse.StatusCode;
            }

            return httpResponseBody;
        }

        public static async Task<dynamic> post(string url, Dictionary<string, string> param, string token)
        {
            HttpClient httpClient = new HttpClient();

            Uri requestUri = new Uri(url);

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Add("x-access-token", token);
            }

            var content = new FormUrlEncodedContent(param);

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the POST request
                httpResponse = await httpClient.PostAsync(requestUri, content);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                return httpResponse.StatusCode;
            }

            return JsonConvert.DeserializeObject<dynamic>(httpResponseBody);
        }

        public static async Task<dynamic> put(string url, Dictionary<string, string> param, string token)
        {
            HttpClient httpClient = new HttpClient();

            Uri requestUri = new Uri(url);

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Add("x-access-token", token);
            }

            var content = new FormUrlEncodedContent(param);

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the POST request
                httpResponse = await httpClient.PutAsync(requestUri, content);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                return httpResponse.StatusCode;
            }

            return JsonConvert.DeserializeObject<dynamic>(httpResponseBody);
        }

        public static async Task<dynamic> delete(string url, string token)
        {
            HttpClient httpClient = new HttpClient();

            Uri requestUri = new Uri(url);

            if (token != null)
            {
                httpClient.DefaultRequestHeaders.Add("x-access-token", token);
            }

            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";

            try
            {
                //Send the POST request
                httpResponse = await httpClient.DeleteAsync(url);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                return httpResponse.StatusCode;
            }

            return JsonConvert.DeserializeObject<dynamic>(httpResponseBody);
        }
    }
}
