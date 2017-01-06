using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Utils
{
    public class Constants
    {
        private static string BASE_URL = "https://kickapi.herokuapp.com";

        public static string getBaseUrl()
        {
            return BASE_URL;
        }
    }
}
