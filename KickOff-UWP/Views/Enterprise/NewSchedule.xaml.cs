using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Repositories;
using KickOff_UWP.Models.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

namespace KickOff_UWP.Views.Enterprise
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewSchedule : Page
    {
        private ObservableCollection<ComboBoxType> comboBoxOptions;

        public NewSchedule()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            loadDataCourt();

            loadingNewSchedule.IsActive = false;
            txtBlockNewSchedule.Visibility = Visibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;

            if (root.CurrentSourcePageType == typeof(NewSchedule))
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        private async void loadDataCourt()
        {
            ObservableCollection<Court> listCourts = await CourtRepository.GetAll();

            comboBoxOptions = new ObservableCollection<ComboBoxType>();

            if (listCourts.Count <= 0)
            {
                return;
            }

            foreach (Court item in listCourts)
            {
                comboBoxOptions.Add(new ComboBoxType(item.name, item.id));
            }

            ComboBoxCourt.ItemsSource = comboBoxOptions;
            ComboBoxCourt.SelectedIndex = 0;
        }

        private void SaveScheduleBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Connection.IsInternet())
            {
                DialogCustom.dialog("Ops...", "Verifique sua conexão.");
                return;
            }

            ComboBoxType combo = ComboBoxCourt.SelectedItem as ComboBoxType;

            if (combo == null || dpDateSchedule.Date == null || tmTimeSchedule.Time == null)
            {
                DialogCustom.dialog("Atenção", "Preencha todos campos corretamente, verifique se você já tem quadras cadastradas para prosseguir.");
                SaveScheduleBtn.IsEnabled = true;
                return;
            }

            loadingNewSchedule.IsActive = true;
            txtBlockNewSchedule.Visibility = Visibility.Visible;
            SaveScheduleBtn.IsEnabled = false;

            DateTime date = dpDateSchedule.Date.DateTime.AddDays(1);

            Schedule schedule = new Schedule("", date.ToString("yyyy-MM-dd"), tmTimeSchedule.Time.ToString(), new Court(combo.value, combo.description, ""));

            var result = ScheduleRepository.Create(schedule);

            Frame.Navigate(typeof(DashboardEnterprise));
        }
    }
}
