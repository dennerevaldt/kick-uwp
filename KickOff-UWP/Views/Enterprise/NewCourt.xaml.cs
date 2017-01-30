﻿using KickOff_UWP.Models.Entities;
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

namespace KickOff_UWP.Views.Enterprise
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewCourt : Page
    {
        private ObservableCollection<ComboBoxType> comboBoxOptions;

        public NewCourt()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            comboBoxOptions = new ObservableCollection<ComboBoxType>();
            comboBoxOptions.Add(new ComboBoxType("Futebol society (7)", "Futebol society (7)"));
            comboBoxOptions.Add(new ComboBoxType("Futebol de salão (Futsal)", "Futebol de salão (Futsal)"));

            ComboBoxTypeCourt.ItemsSource = comboBoxOptions;
            ComboBoxTypeCourt.SelectedIndex = 0;

            loadingNewCourt.IsActive = false;
            txtBlockNewCourt.Visibility = Visibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;

            if (root.CurrentSourcePageType == typeof(NewCourt))
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        private void SaveCourtBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!Connection.IsInternet())
            {
                DialogCustom.dialog("Ops...", "Verifique sua conexão.");
                return;
            }

            loadingNewCourt.IsActive = true;
            txtBlockNewCourt.Visibility = Visibility.Visible;
            SaveCourtBtn.IsEnabled = false;

            ComboBoxType combo = ComboBoxTypeCourt.SelectedItem as ComboBoxType;

            if (combo == null || txtBoxName.Text == "")
            {
                DialogCustom.dialog("Atenção", "Preencha todos campos corretamente");
                SaveCourtBtn.IsEnabled = true;
                return;
            }

            Court court = new Court("", txtBoxName.Text, combo.description);

            var result = CourtRepository.Create(court);

            Frame.Navigate(typeof(DashboardEnterprise));
        }
    }
}
