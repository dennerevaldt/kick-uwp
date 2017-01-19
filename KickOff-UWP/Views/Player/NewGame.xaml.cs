using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Repositories;
using KickOff_UWP.Models.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
        private ObservableCollection<ComboBoxType> comboBoxOptions;

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

            ComboBoxType combo = ComboBoxSchedules.SelectedItem as ComboBoxType;

            if (combo == null)
            {
                DialogCustom.dialog("Atenção", "Preencha todos campos corretamente");
                SaveGameBtn.IsEnabled = true;
                return;
            }
        }

        private async void getProximity()
        {
            dynamic list = await EnterpriseRepository.GetProximity(new LatLng("-29.4753", "-49.9849"));
        }
    }
}
