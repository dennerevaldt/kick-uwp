using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Repositories;
using KickOff_UWP.Models.Utils;
using KickOff_UWP.Views.AuthRegister;
using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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
        ObservableCollection<Schedule> listSchedule = new ObservableCollection<Schedule>();
        Schedule scheduleSelected;

        public DashboardEnterprise()
        {
            this.InitializeComponent();
            PivotDashEnterprise.Title = "Dashboard empresa";

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            Window.Current.Activate();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            App.Current.Exit();
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
            if (!Connection.IsInternet())
            {
                DialogCustom.dialog("Ops...", "Verifique sua conexão.");
                return;
            }

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

                    CourtEmpty.Visibility = listCourt.Count <= 0 ? Visibility.Visible : Visibility.Collapsed;

                    loadingCourt.IsActive = false;

                    break;
                case 1:
                    AppBar.Visibility = Visibility.Visible;
                    AddCourtBtn.Visibility = Visibility.Collapsed;
                    AddScheduleBtn.Visibility = Visibility.Visible;

                    listSchedule.Clear();

                    loadingSchedule.IsActive = true;

                    listSchedule = await ScheduleRepository.GetAll();
                    scheduleList.ItemsSource = listSchedule;

                    ScheduleEmpty.Visibility = listSchedule.Count <= 0 ? Visibility.Visible : Visibility.Collapsed;

                    loadingSchedule.IsActive = false;
                    
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

                    CourtEmpty.Visibility = listCourt.Count <= 0 ? Visibility.Visible : Visibility.Collapsed;
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
            bool confirm = await DialogCustom.confirm("Confirmação", "Deseja realmente remover esta quadra?");

            if (!confirm)
            {
                return;
            }

            listCourt.Remove(courtSelected);
            CourtEmpty.Visibility = listCourt.Count <= 0 ? Visibility.Visible : Visibility.Collapsed;

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

        private void scheduleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Frame.Navigate(typeof(EditSchedule), scheduleList.SelectedItem);
        }

        private void GridScheduleList_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;

            scheduleSelected = senderElement.DataContext as Schedule;

            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private async void DeleteButtonSchedule_Click(object sender, RoutedEventArgs e)
        {
            bool confirm = await DialogCustom.confirm("Confirmação", "Deseja realmente remover este horário?");

            if (!confirm)
            {
                return;
            }

            listSchedule.Remove(scheduleSelected);
            ScheduleEmpty.Visibility = listSchedule.Count <= 0 ? Visibility.Visible : Visibility.Collapsed;

            var result = await ScheduleRepository.Delete(scheduleSelected);

            if (result == null)
            {
                DialogCustom.dialog("Ops :(", "Estamos com problemas, tente novamente mais tarde");
            }
        }
    }
}
