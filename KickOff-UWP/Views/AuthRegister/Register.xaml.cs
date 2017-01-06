using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Repositories;
using KickOff_UWP.Models.Utils;
using System.Collections.Generic;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
            Window.Current.Activate();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
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

            if (pivot == 0) //enterprise
            {
                if (txtBoxEnterpriseFullname.Text == "" || txtBoxEnterpriseEmail.Text == "" || txtBoxEnterpriseUsername.Text == "" || txtBoxEnterprisePwd.Password == "" || txtBoxEnterpriseTelephone.Text == "" || place.latLng == null)
                {
                    DialogCustom.dialog("Atenção", "Preencha todos os campos corretamente");
                }
                else
                {
                    // create enterprise
                    Models.Entities.Enterprise enterprise = new Models.Entities.Enterprise("", txtBoxEnterpriseFullname.Text, txtBoxEnterpriseUsername.Text, txtBoxEnterpriseEmail.Text, txtBoxEnterprisePwd.Password, place.description, place.latLng.lat, place.latLng.lng, "", txtBoxEnterpriseTelephone.Text);
                    Models.Entities.Enterprise result = await EnterpriseRepository.Create(enterprise);

                    if (result != null)
                    {
                        DialogCustom.dialog("Parabéns", "Cadastrado com sucesso!");
                    } else
                    {
                        DialogCustom.dialog("Ops :(", "Estamos com problemas, tente novamente");
                    }
                }
            }

            if (pivot == 1) //player
            {
                if (txtBoxPlayerFullname.Text == "" || txtBoxPlayerEmail.Text == "" || txtBoxPlayerUsername.Text == "" || txtBoxPlayerPwd.Password == "" || txtBoxPlayerPosition.Text == "" || place.latLng == null)
                {
                    DialogCustom.dialog("Atenção", "Preencha todos os campos corretamente");
                }
                else
                {
                    // create player
                    Models.Entities.Player player = new Models.Entities.Player("", txtBoxPlayerFullname.Text, txtBoxPlayerUsername.Text, txtBoxPlayerEmail.Text, txtBoxPlayerPwd.Password, place.description, place.latLng.lat, place.latLng.lng, "", txtBoxPlayerPosition.Text);
                    Models.Entities.Player result = await PlayerRepository.Create(player);

                    if (result != null)
                    {
                        DialogCustom.dialog("Parabéns", "Cadastrado com sucesso!");
                    } else
                    {
                        DialogCustom.dialog("Ops :(", "Estamos com problemas, tente novamente");
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

                    if (data.Count > 0)
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
    }
}
