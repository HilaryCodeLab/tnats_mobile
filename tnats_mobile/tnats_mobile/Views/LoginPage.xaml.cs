using System;
using System.Threading.Tasks;
using tnats_mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tnats_mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        /// <summary>
        /// CONSTRUCTOR CLASS
        /// </summary>
        public LoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// "LOGIN" CLICK EVENT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LoginProcedure(object sender, EventArgs e)
        {
            var token = App.ApiService.Login(Entry_Username.Text, Entry_Password.Text);

            if (!string.IsNullOrEmpty(token))
            {
                Application.Current.MainPage = new HomePage();

                Task.Run(() => { new ApiServices().GetSpecies(token); });

                Task.Run(() => { new ApiServices().GetLocations(token); });
            }
            else
            {
                messageLabel.Text = "Invalid username and/or password!";
                Entry_Password.Text = string.Empty;
            }
        }
    }
}