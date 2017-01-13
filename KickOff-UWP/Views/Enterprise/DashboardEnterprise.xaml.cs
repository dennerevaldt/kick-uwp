using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Repositories;
using KickOff_UWP.Models.Utils;
using KickOff_UWP.Views.AuthRegister;
using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using winsdkfb;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KickOff_UWP.Views.Enterprise
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardEnterprise : Page
    {
        ObservableCollection<Court> listCourt = new ObservableCollection<Court>();
        Court courtSelected;

        public DashboardEnterprise()
        {
            this.InitializeComponent();
            PivotDashEnterprise.Title = "Dashboard empresa";

            Window.Current.Activate();          
        }

        private async void btnLogoutEnterprise_Click(object sender, RoutedEventArgs e)
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

        private async void pivotEnterprise_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch (PivotDashEnterprise.SelectedIndex)
            {
                case 0:
                    AppBar.Visibility = Visibility.Visible;
                    AddCourtBtn.Visibility = Visibility.Visible;
                    AddScheduleBtn.Visibility = Visibility.Collapsed;

                    listCourt.Clear();

                    loadingCourt.IsActive = true;

                    listCourt = await CourtRepository.GetAll();
                    courtList.ItemsSource = listCourt;

                    loadingCourt.IsActive = false;

                    break;
                case 1:
                    AppBar.Visibility = Visibility.Visible;
                    AddCourtBtn.Visibility = Visibility.Collapsed;
                    AddScheduleBtn.Visibility = Visibility.Visible;
                    break;
                case 2:
                    AppBar.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    AppBar.Visibility = Visibility.Collapsed;

                    var localSettings = ApplicationData.Current.LocalSettings;
                    txtBoxNameUser.Text = localSettings.Values["fullname"] as string;
                    txtBoxEmailUser.Text = localSettings.Values["email"] as string;

                    break;
            }
        }

        private void AddCourtBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewCourt));
        }

        private void AddScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(NewSchedule));
        }

        private void GridCourtList_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;

            courtSelected = senderElement.DataContext as Court;

            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private async void DeleteButtonCourt_Click(object sender, RoutedEventArgs e)
        {
            bool confirm = await DialogCustom.confirm("Confirmação", "Deseja realmente remover este elemento?");

            if (!confirm)
            {
                return;
            }

            listCourt.Remove(courtSelected);
            var result = await CourtRepository.Delete(courtSelected);

            if (result == null)
            {
                DialogCustom.dialog("Ops :(", "Estamos com problemas, tente novamente mais tarde");
            } 
        }

        private void courtList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(EditCourt), courtList.SelectedItem);
        }
    }
}
