using KickOff_UWP.Models.Entities;
using KickOff_UWP.Models.Repositories;
using KickOff_UWP.Models.Utils;
using KickOff_UWP.Views.Enterprise;
using KickOff_UWP.Views.Player;
using Newtonsoft.Json;
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
using winsdkfb;
using winsdkfb.Graph;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KickOff_UWP.Views.AuthRegister
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConfirmAccount : Page
    {
        FBUser user = null;
        Place place = new Place();
        private ObservableCollection<ComboBoxType> comboBoxOptions;

        public ConfirmAccount()
        {
            this.InitializeComponent();

            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            Window.Current.Activate();

            comboBoxOptions = new ObservableCollection<ComboBoxType>();
            comboBoxOptions.Add(new ComboBoxType("Empresa", "1"));
            comboBoxOptions.Add(new ComboBoxType("Jogador", "2"));

            ComboBoxTypeAccount.ItemsSource = comboBoxOptions;
            ComboBoxTypeAccount.SelectedIndex = 0;

            setLoading(false);
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;

            if (root.CurrentSourcePageType == typeof(ConfirmAccount))
            {
                Frame.GoBack();
                e.Handled = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as FBUser;
            string gender = user.Gender == "Male" ? "vindo" : "vinda";
            txtBoxName.Text = "Olá seja bem "+ gender + " " + user.Name + ", precisamos confirmar apenas mais alguns dados para seu cadastro.";
        }

        private async void SaveNewUser(object sender, RoutedEventArgs e)
        {
            if (!Connection.IsInternet())
            {
                DialogCustom.dialog("Ops...", "Verifique sua conexão.");
                return;
            } 

            ComboBoxType type = ComboBoxTypeAccount.SelectedItem as ComboBoxType;

            if (type.value == "1")
            {
                if (user == null || txtBoxUsername.Text == "" || txtBoxTelephone.Text == "" || place.latLng == null)
                {
                    DialogCustom.dialog("Atenção", "Preencha todos os campos corretamente");
                }
                else
                {
                    setLoading(true);

                    // create enterprise
                    Models.Entities.Enterprise enterprise = new Models.Entities.Enterprise("", user.Name, txtBoxUsername.Text, user.Email, user.Id, place.description, place.latLng.lat, place.latLng.lng, "", txtBoxTelephone.Text);
                    await EnterpriseRepository.Create(enterprise);

                    if (enterprise != null)
                    {
                        await authenticUser(enterprise.userName, user.Id);
                        setLoading(false);
                    }
                    else
                    {
                        DialogCustom.dialog("Ops :(", "Estamos com problemas, tente novamente");
                        setLoading(false);
                    }
                }
            }

            if (type.value == "2")
            {
                if (user == null || txtBoxUsername.Text == "" || txtBoxPosition.Text == "" || place.latLng.lat == null)
                {
                    DialogCustom.dialog("Atenção", "Preencha todos os campos corretamente");
                }
                else
                {
                    setLoading(true);

                    // create player
                    Models.Entities.Player player = new Models.Entities.Player("", user.Name, txtBoxUsername.Text, user.Email, user.Id, place.description, place.latLng.lat, place.latLng.lng, "", txtBoxPosition.Text);
                    await PlayerRepository.Create(player);

                    if (player != null)
                    {
                        await authenticUser(player.userName, user.Id);
                        setLoading(false);
                    }
                    else
                    {
                        DialogCustom.dialog("Ops :(", "Estamos com problemas, tente novamente");
                        setLoading(false);
                    }
                }
            }
        }

        private async void logoutFB()
        {
            FBSession sess = FBSession.ActiveSession;
            if (sess != null)
            {
                await sess.LogoutAsync();
            }
        }

        private void ComboBoxTypeAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxType type = ComboBoxTypeAccount.SelectedItem as ComboBoxType;

            if (type.value == "1")
            {
                txtBoxTelephone.Visibility = Visibility.Visible;
                txtBoxPosition.Visibility = Visibility.Collapsed;
            }

            if (type.value == "2")
            {
                txtBoxTelephone.Visibility = Visibility.Collapsed;
                txtBoxPosition.Visibility = Visibility.Visible;
            }
        }

        private async void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (place != null)
            {
                place = new Place();
            }

            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                List<Place> data = await PlaceAPI.autocomplete(AutoSugCity.Text);

                if (data.Count > 0)
                {
                    PlaceDetails.Visibility = Visibility.Visible;
                    NoResults.Visibility = Visibility.Collapsed;
                }
                else
                {
                    PlaceDetails.Visibility = Visibility.Collapsed;
                    NoResults.Visibility = Visibility.Visible;
                }

                //Set the ItemsSource to be your filtered dataset
                sender.ItemsSource = data;
            }
        }

        private async void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            place = (Place)args.SelectedItem;

            if (args.SelectedItem != null)
            {
                AutoSugCity.Text = place.description;
                place = await PlaceAPI.latLng(place);
            }
        }

        private void setLoading(bool isLoading)
        {
            txtBlockLoad.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            prgLoad.IsActive = isLoading;
        }

        private async Task authenticUser(string username, string password)
        {
            try
            {
                string token = await AuthRepository.login(username, password);
                setLoading(false);

                if (token == "")
                {
                    DialogCustom.dialog("Ops...", "Usuário ou senha estão incorretos.");
                    return;
                }

                dynamic data = JsonConvert.DeserializeObject(await AuthRepository.getDataUSer(token));
                AuthRepository.setCredentials(token, data);
                setLoading(false);
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
    }
}
