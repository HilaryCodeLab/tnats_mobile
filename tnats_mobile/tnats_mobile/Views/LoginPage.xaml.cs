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
        public LoginPage()
        {
            InitializeComponent();

            //bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();

            //if (isAvailable)
            //{
            //    Debug.WriteLine("network is available");C:\Users\marce\Documents\Source Tree\tnats_mobile\tnats_mobile\tnats_mobile\Views\AboutPage.xaml
            //}

            //else
            //{
            //    Debug.WriteLine("network is unavailable");
            //}
        }

        async void LoginProcedure(object sender, EventArgs e)
        {
            var token = App.ApiService.Login(Entry_Username.Text, Entry_Password.Text);

            if (!string.IsNullOrEmpty(token))
            {
                new ApiServices().GetSpecies();

                new ApiServices().GetLocations();

                await Navigation.PushAsync(new HomePage());
                //await Navigation.PopAsync();
                //var n = Navigation.NavigationStack;

                //if (n.Count == 0)
                //{ 

                //    await Navigation.PushAsync(new NavigationPage(new HomePage()));
                //    //Navigation.InsertPageBefore(new HomePage(), this);
                //    //await Navigation.PopAsync();
                //}
                //else
                //{
                //    await Navigation.PopToRootAsync();
                //}
            }
            else
            {
                messageLabel.Text = "Invalid username and/or password!";
                Entry_Password.Text = string.Empty;
            }
        }
    }
}