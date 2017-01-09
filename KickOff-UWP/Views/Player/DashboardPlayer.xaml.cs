using KickOff_UWP.Models.Repositories;
using KickOff_UWP.Views.AuthRegister;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using winsdkfb;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KickOff_UWP.Views.Player
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardPlayer : Page
    {
        public DashboardPlayer()
        {
            this.InitializeComponent();
            PivotDashPlayer.Title = "Dashboard jogador";

            Window.Current.Activate();

            var localSettings = ApplicationData.Current.LocalSettings;
            txtBoxNameUser.Text = localSettings.Values["fullname"] as string;
            txtBoxEmailUser.Text = localSettings.Values["email"] as string;
        }

        private async void btnLogoutPlayer_Click(object sender, RoutedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            string user = localSettings.Values["fullname"] as string;
            AuthRepository.clearCredentials(user);

            FBSession sess = FBSession.ActiveSession;
            if (sess != null)
            {
                await sess.LogoutAsync();
            }

            Frame.Navigate(typeof(Login));
        }
    }
}
