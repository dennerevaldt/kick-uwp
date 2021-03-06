﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KickOff_UWP.Models.Utils
{
    public class DialogCustom
    {
        public static async void dialog(string title, string msg)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(
                msg,
                title);

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });

            var result = await dialog.ShowAsync();
        }

        public static async Task<bool> confirm(string title, string msg)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(msg, title);

            dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });
            dialog.Commands.Add(new Windows.UI.Popups.UICommand("Cancelar") { Id = 1 });

            var result = await dialog.ShowAsync();

            return (int)result.Id == 0 ? true : false;
        }
    }
}
