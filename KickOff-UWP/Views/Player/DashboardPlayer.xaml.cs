using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Repositories;
using KickOff_UWP.Views.AuthRegister;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
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
        ObservableCollection<Game> listGames = new ObservableCollection<Game>();
        //Game gameSelected;

        public DashboardPlayer()
        {
            this.InitializeComponent();
            PivotDashPlayer.Title = "Dashboard jogador";

            Window.Current.Activate();
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

        private async void PivotDashPlayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot pivot = sender as Pivot;
            PivotItem pivotItemSelected = ((PivotItem)((Pivot)sender).SelectedItem);

            for (int i = 0; i < pivot.Items.Count; i++)
            {
                PivotItem pivotItem = pivot.Items[i] as PivotItem;
                TextBlock tb = pivotItem.Header as TextBlock;
                if (pivotItem == pivotItemSelected)
                {
                    //Style 
                    tb.Foreground = new SolidColorBrush(Colors.DarkGreen);
                }
                else
                {
                    tb.Foreground = new SolidColorBrush(Colors.Gray);
                }
            }

            switch (PivotDashPlayer.SelectedIndex)
            {
                case 0:
                    AppBar.Visibility = Visibility.Visible;

                    listGames.Clear();

                    loadingGame.IsActive = true;

                    listGames = await GameRepository.GetAll();
                    gameList.ItemsSource = listGames;

                    GameEmpty.Visibility = listGames.Count <= 0 ? Visibility.Visible : Visibility.Collapsed;

                    loadingGame.IsActive = false;

                    break;
                case 1:
                    AppBar.Visibility = Visibility.Collapsed;
                    GamesProximity.Visibility = Visibility.Visible;
                    break;
                case 2:
                    AppBar.Visibility = Visibility.Collapsed;
                    NotificationsEmpty.Visibility = Visibility.Visible;
                    break;
                case 3:
                    AppBar.Visibility = Visibility.Collapsed;
                    var localSettings = ApplicationData.Current.LocalSettings;
                    txtBoxNameUser.Text = localSettings.Values["fullname"] as string;
                    txtBoxEmailUser.Text = localSettings.Values["email"] as string;

                    break;
            }
        }

        private void AddGameBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewGame));
        }
    }
}
