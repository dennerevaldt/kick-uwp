using KickOff_UWP.Models.Utils;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Windows.Security.Credentials;

namespace KickOff_UWP.Models.Repositories
{
    public class AuthRepository
    {
        public static async Task<string> login(string username, string password)
        {  
            string url = Constants.getBaseUrl() + "/token";
             
            var values = new Dictionary<string, string>();
            values.Add("username", username);
            values.Add("password", password);

            dynamic result = await HTTP.post(url, values, null);

            try
            {
                if (result.token != null)
                {
                    return result.token;
                }
                else
                {
                    return "500";
                }
            }
            catch (Exception)
            {
                if (result == HttpStatusCode.NotFound)
                {
                    return "404";
                }
                else
                {
                    return "500";
                }
            }
        }

        public static async Task<string> loginFacebook(string email, string password)
        {
            string url = Constants.getBaseUrl() + "/token-facebook";

            var values = new Dictionary<string, string>();
            values.Add("email", email);
            values.Add("password", password);

            dynamic result = await HTTP.post(url, values, null);


            try
            {
                if (result.token != null)
                {
                    return result.token;
                } else
                {
                    return "500";
                }
            }
            catch (Exception)
            {
                if (result == HttpStatusCode.NotFound)
                {
                    return "404";
                }
                else
                {
                    return "500";
                }
            }             
        }

        public static async Task<dynamic> getDataUSer(string token)
        {
            string url = Constants.getBaseUrl() + "/user";

            return await HTTP.get(url, token);
        }

        public static void setCredentials(string token, dynamic data)
        {
            var cofre = new PasswordVault();
            string name = (string)data.Person.fullname;
            cofre.Add(new PasswordCredential("winKickApp", name, token));
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["email"] = (string)data.Person.email;
            localSettings.Values["typeperson"] = (string) data.Person.typeperson;
            localSettings.Values["fullname"] = name;
            localSettings.Values["id"] = (string) data.Person.id;
        }

        public static void clearCredentials(string user)
        {
            var cofre = new PasswordVault();
            var credential = cofre.Retrieve("winKickApp", user);
            credential.RetrievePassword();
            cofre.Remove(credential);
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values.Remove("email");
            localSettings.Values.Remove("typeperson");
            localSettings.Values.Remove("fullname");
            localSettings.Values.Remove("id");
        }

        public static string getCredentials(string user)
        {
            var cofre = new PasswordVault();
            return cofre.Retrieve("winKickApp", user).Password;
        }
    }
}
