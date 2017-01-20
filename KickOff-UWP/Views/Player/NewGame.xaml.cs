using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Repositories;
using KickOff_UWP.Models.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KickOff_UWP.Views.Player
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewGame : Page
    {
        // private ObservableCollection<ComboBoxType> comboBoxOptions;
        ObservableCollection<Models.Entities.Enterprise> listEnterprises;
        ObservableCollection<Models.Entities.Schedule> listSchedules;

        public NewGame()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            loadingNewGame.IsActive = false;
            txtBlockNewGame.Visibility = Visibility.Collapsed;

            getProximity();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        private void SaveGameBtn_Click(object sender, RoutedEventArgs e)
        {
            loadingNewGame.IsActive = true;
            txtBlockNewGame.Visibility = Visibility.Visible;
            SaveGameBtn.IsEnabled = false;

            Schedule comboSchedule = ComboBoxSchedules.SelectedItem as Schedule;
            Models.Entities.Enterprise comboEnterprise = ComboBoxEnterprises.SelectedItem as Models.Entities.Enterprise;

            if (comboSchedule == null || comboEnterprise == null)
            {
                DialogCustom.dialog("Atenção", "Preencha todos campos corretamente");
                SaveGameBtn.IsEnabled = true;
                loadingNewGame.IsActive = false;
                txtBlockNewGame.Visibility = Visibility.Collapsed;
                return;
            }
        }

        private async void getProximity()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                    Geolocator geolocator = new Geolocator();

                    // Carry out the operation.
                    Geoposition pos = await geolocator.GetGeopositionAsync();

                    string lat = pos.Coordinate.Point.Position.Latitude.ToString().Substring(0, 7).Replace(",", ".");
                    string lng = pos.Coordinate.Point.Position.Longitude.ToString().Substring(0, 7).Replace(",", ".");

                    dynamic list = await EnterpriseRepository.GetProximity(new LatLng(lat, lng));

                    listEnterprises = new ObservableCollection<Models.Entities.Enterprise>();

                    foreach (var item in list)
                    {
                        listEnterprises.Add(new Models.Entities.Enterprise(
                            item["Person"]["id"].ToString(),
                            item["Person"]["fullname"].ToString(),
                            item["Person"]["username"].ToString(),
                            item["Person"]["email"].ToString(),
                            item["Person"]["password"].ToString(),
                            item["Person"]["district"].ToString(),
                            item["Person"]["lat"].ToString(),
                            item["Person"]["lng"].ToString(),
                            item["id"].ToString(),
                            item["telephone"].ToString())
                        );
                    }

                    ComboBoxEnterprises.ItemsSource = listEnterprises;
                    ComboBoxEnterprises.SelectedIndex = 0;

                    break;

                case GeolocationAccessStatus.Denied:
                    DialogCustom.dialog("Permissão", "Estamos sem permissão para acessar sua localização, verifique e tente novamente.");
                    break;

                case GeolocationAccessStatus.Unspecified:
                    DialogCustom.dialog("Permissão", "Estamos sem permissão para acessar sua localização, verifique e tente novamente.");
                    break;
            }
        }

        private async void ComboBoxEnterprises_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Models.Entities.Enterprise comboEnterprise = ComboBoxEnterprises.SelectedItem as Models.Entities.Enterprise;
            listSchedules = await ScheduleRepository.GetAllById(comboEnterprise.idEnterprise);
            ComboBoxSchedules.ItemsSource = listSchedules;

            if (listSchedules.Count > 0)
            {
                ComboBoxSchedules.SelectedIndex = 0;
            }
        }
    }
}
