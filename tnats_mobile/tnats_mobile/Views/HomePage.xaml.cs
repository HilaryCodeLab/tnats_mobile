using Plugin.Media;
using System;
using System.IO;
using System.Threading.Tasks;
using tnats_mobile.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tnats_mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public string token { get; set; }

        /// <summary>
        /// CONSTRUCTOR CLASS OF HOME PAGE
        /// </summary>
        public HomePage()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                var user = await App.Database.GetLoggedUser();

                if (user == null)
                    Application.Current.MainPage = new LoginPage();
                else
                {
                    token = user.Token;

                    var obsList = await App.Database.GetItemsAsync();

                    if (obsList.Count > 0)
                        if (DependencyService.Get<INetworkAvailable>().IsNetworkAvailable())
                            foreach (var item in obsList)
                                await Task.Run(() => new ApiServices().SaveObservation(item, token));
                }
            });
        }

        /// <summary>
        /// "LOGOUT" CLICK EVENT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.Database.DeleteUser();
            Application.Current.MainPage = new LoginPage();
        }

        /// <summary>
        /// "TAKE PICTURE" CLICK EVENT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        async void AddNewItemClicked(object sender, EventArgs e)
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
                Directory = "",
                Name = $"Pic_{DateTime.Now.ToString("ddMMyyyyHHmmss")}.jpg"
            });

            if (file == null)
                return;

            Application.Current.MainPage = new NewItemPage(file, token);
        }
    }
}