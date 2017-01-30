using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Repositories;
using KickOff_UWP.Models.Utils;
using KickOff_UWP.Views.Enterprise;
using KickOff_UWP.Views.Player;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KickOff_UWP.Views.AuthRegister
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Register : Page
    {
        Place place = new Place();

        public Register()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            setLoading(false, 0);
            setLoading(false, 1);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;

            if (root.CurrentSourcePageType == typeof(Register))
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        private async void SaveNewUser(object sender, RoutedEventArgs e)
        {
            if (!Connection.IsInternet())
            {
                DialogCustom.dialog("Ops...", "Verifique sua conexão.");
                return;
            }

            int pivot = PivotUser.SelectedIndex;

            setLoading(true, pivot);

            if (pivot == 0) //enterprise
            {
                if (txtBoxEnterpriseFullname.Text == "" || txtBoxEnterpriseEmail.Text == "" || txtBoxEnterpriseUsername.Text == "" || txtBoxEnterprisePwd.Password == "" || txtBoxEnterpriseTelephone.Text == "" || place.latLng == null)
                {
                    DialogCustom.dialog("Atenção", "Preencha todos os campos corretamente");
                    setLoading(false, pivot);
                }
                else
                {
                    // create enterprise
                    Models.Entities.Enterprise enterprise = new Models.Entities.Enterprise("", txtBoxEnterpriseFullname.Text, txtBoxEnterpriseUsername.Text, txtBoxEnterpriseEmail.Text, txtBoxEnterprisePwd.Password, place.description, place.latLng.lat, place.latLng.lng, "", txtBoxEnterpriseTelephone.Text);
                    var result = await EnterpriseRepository.Create(enterprise);

                    if (result != null)
                    {
                        await authenticUser(enterprise.userName, enterprise.password);
                    } else
                    {
                        DialogCustom.dialog("Ops :(", "Estamos com problemas, verifique os dados e tente novamente");
                        setLoading(false, pivot);
                    }
                }
            }

            if (pivot == 1) //player
            {
                if (txtBoxPlayerFullname.Text == "" || txtBoxPlayerEmail.Text == "" || txtBoxPlayerUsername.Text == "" || txtBoxPlayerPwd.Password == "" || txtBoxPlayerPosition.Text == "" || place.latLng == null)
                {
                    DialogCustom.dialog("Atenção", "Preencha todos os campos corretamente");
                    setLoading(false, pivot);
                }
                else
                {
                    // create player
                    Models.Entities.Player player = new Models.Entities.Player("", txtBoxPlayerFullname.Text, txtBoxPlayerUsername.Text, txtBoxPlayerEmail.Text, txtBoxPlayerPwd.Password, place.description, place.latLng.lat, place.latLng.lng, "", txtBoxPlayerPosition.Text);
                    var result = await PlayerRepository.Create(player);

                    if (result != null)
                    {
                        await authenticUser(player.userName, player.password);
                    } else
                    {
                        DialogCustom.dialog("Ops :(", "Estamos com problemas, verifique os dados e tente novamente");
                        setLoading(false, pivot);
                    }               
                }
            }
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (place != null)
            {
                place = new Place();
            }

            int pivot = PivotUser.SelectedIndex;

            if (pivot == 0)
            {
                if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    List<Place> data = await PlaceAPI.autocomplete(AutoSugCityEnterprise.Text);

                    if (data != null && data.Count > 0)
                    {
                        PlaceDetailsEnterprise.Visibility = Visibility.Visible;
                        NoResultsEnterprise.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        PlaceDetailsEnterprise.Visibility = Visibility.Collapsed;
                        NoResultsEnterprise.Visibility = Visibility.Visible;
                    }

                    //Set the ItemsSource to be your filtered dataset
                    sender.ItemsSource = data;
                }
            }

            if (pivot == 1)
            {
                if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                {
                    List<Place> data = await PlaceAPI.autocomplete(AutoSugCityPlayer.Text);

                    if (data != null)
                    {
                        if (data.Count > 0)
                        {
                            PlaceDetailsPlayer.Visibility = Visibility.Visible;
                            NoResultsPlayer.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            PlaceDetailsPlayer.Visibility = Visibility.Collapsed;
                            NoResultsPlayer.Visibility = Visibility.Visible;
                        }
                    }
                    
                    //Set the ItemsSource to be your filtered dataset
                    sender.ItemsSource = data;
                }
            }

            
        }

        private async void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem != null)
            {
                place = (Place) args.SelectedItem;
                int pivot = PivotUser.SelectedIndex;
                // User selected an item from the suggestion list, take an action on it here.

                if (pivot == 0)
                {
                    AutoSugCityEnterprise.Text = place.description;
                    place = await PlaceAPI.latLng(place);
                }

                if (pivot == 1)
                {
                    AutoSugCityPlayer.Text = place.description;
                    place = await PlaceAPI.latLng(place);
                }
            }
        }

        private async Task authenticUser(string username, string password)
        {
            try
            {
                string token = await AuthRepository.login(username, password);

                switch (token)
                {
                    case "":
                        DialogCustom.dialog("Ops...", "Usuário ou senha estão incorretos.");
                        setLoading(false, 0);
                        setLoading(false, 1);
                        return;
                    case "404":
                        DialogCustom.dialog("Ops...", "Usuário incorreto ou não existe.");
                        setLoading(false, 0);
                        setLoading(false, 1);
                        return;
                    case "500":
                        DialogCustom.dialog("Ops...", "Usuário ou senha estão incorretos.");
                        setLoading(false, 0);
            setLoading(false, 1);
                        return;
                    default:
                        break;
                }

                dynamic data = JsonConvert.DeserializeObject(await AuthRepository.getDataUSer(token));
                AuthRepository.setCredentials(token, data);

                var typeUser = (string)data.Person.typeperson;
                if (typeUser == "P")
                {
                    // dash player
                    Frame.Navigate(typeof(DashboardPlayer));
                }
                else
                {
                    // dash enterprise
                    Frame.Navigate(typeof(DashboardEnterprise));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void setLoading(bool isLoading, int pivot)
        {
            if (pivot == 0)
            {
                txtBlockLoadEn.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
                prgLoadEn.IsActive = isLoading;
                AddItemAppBarBtn.IsEnabled = !isLoading;
            }

            if (pivot == 1)
            {
                txtBlockLoadPl.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
                prgLoadPl.IsActive = isLoading;
                AddItemAppBarBtn.IsEnabled = !isLoading;
            }
            
        }

        private void PivotUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
        }
    }
}
