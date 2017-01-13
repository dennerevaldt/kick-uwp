using KickOff_UWP.Models.Repositories;
using KickOff_UWP.Models.Utils;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using winsdkfb;
using winsdkfb.Graph;
using KickOff_UWP.Views.Player;
using KickOff_UWP.Views.Enterprise;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KickOff_UWP.Views.AuthRegister
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Login : Page
    {
        public Login()
        {
            this.InitializeComponent();
            setLoading(false);
        }

        private async void button_Login(object sender, RoutedEventArgs e)
        {
            string username = user.Text;
            string password = pass.Password;

            if (!Connection.IsInternet())
            {
                DialogCustom.dialog("Ops...", "Verifique sua conexão.");
                return;
            }

            if (username == "" || password == "")
            {
                DialogCustom.dialog("Ops...", "Usuário e senha devem ser preenchidos corretamente.");
                return;
            }

            setLoading(true);

            try
            {
                string token = await AuthRepository.login(username, password);
                setLoading(false);

                switch (token)
                {
                    case "":
                        DialogCustom.dialog("Ops...", "Usuário ou senha estão incorretos.");
                        return;
                    case "404":
                        DialogCustom.dialog("Ops...", "Usuário incorreto ou não existe.");
                        return;
                    case "500":
                        DialogCustom.dialog("Ops...", "Usuário ou senha estão incorretos.");
                        return;
                    default:
                        break;
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

        private async void button_LoginFace(object sender, RoutedEventArgs e)
        {
            if (!Connection.IsInternet())
            {
                DialogCustom.dialog("Ops...", "Verifique sua conexão.");
                return;
            }

            setLoading(true);

            FBSession sess = FBSession.ActiveSession;
            sess.FBAppId = "545567282282477";
            sess.WinAppId = "58a5039d16fb4a24924f2acf16c672c7";
            List<String> permissionList = new List<String>();
            permissionList.Add("public_profile");
            permissionList.Add("email");

            FBPermissions permissions = new FBPermissions(permissionList);

            // Login to Facebook
            FBResult result = await sess.LoginAsync(permissions);
            if (result.Succeeded)
            {
                FBUser user = sess.User;
                string token = await AuthRepository.loginFacebook(user.Email, user.Id);

                if (token == "404")
                {
                    //confirm
                    Frame.Navigate(typeof(ConfirmAccount), user);
                    return;
                }
                else if (token == "500")
                {
                    //error
                    DialogCustom.dialog("Ops :/", "Estamos com problemas, tente mais tarde");
                }          
                  
                dynamic data = await AuthRepository.getDataUSer(token);
                AuthRepository.setCredentials(token, data);

                setLoading(false);
                var typeUser =(string) data.Person.typeperson;
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
            else
            {
                setLoading(false);
                DialogCustom.dialog("Ops...", "Não foi possível autenticar via Facebook, tente novamente.");
            }
        }

        private void setLoading (bool isLoading)
        {
            txtBlockLoad.Visibility = isLoading ? Visibility.Visible : Visibility.Collapsed;
            prgLoad.IsActive = isLoading;
            btnLogin.IsEnabled = !isLoading;
            btnLoginFace.IsEnabled = !isLoading;
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Register));
        }
    }
}
