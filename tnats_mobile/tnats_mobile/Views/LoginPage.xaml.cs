using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tnats_mobile.Models;
using tnats_mobile.Util;
using tnats_mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tnats_mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
        }

        async void LoginProcedure(object sender, EventArgs e)
        {
            string userName = Entry_Username.Text;
            string password = Entry_Password.Text;

            var isValid = AreCredentialsCorrect(userName, password);
            if (isValid)
            {
                bool doCredentialsExist = App.CredentialsService.DoCredentialsExist();
                if (!doCredentialsExist)
                {
                    App.CredentialsService.SaveCredentials(userName, password);
                }

                Navigation.InsertPageBefore(new HomePage(), this);
                await Navigation.PopAsync();
            }
            else
            {
                messageLabel.Text = "Login failed";
                Entry_Password.Text = string.Empty;
            }
            //User user = new User(Entry_Username.Text, Entry_Password.Text);
            //if (user.CheckInformation())
            //{
            //    DisplayAlert("Success", "Login Success", "OK");
            //    var result = await App.RestService.Login(user);
            //    //var result = App.ApiService.Login("test@test.com.au", "123123123");
            //    if (result.Access_token != null)
            //    {
            //        App.UserDatabase.SaveUser(user);
            //    }

            //}
            //else
            //{
            //    DisplayAlert("Error", "Login credentials are incorrect", "OK");
            //}
        }

        bool AreCredentialsCorrect(string username, string password)
        {
            return username == User_Constants.Username && password == User_Constants.Password;
        }
    }
}