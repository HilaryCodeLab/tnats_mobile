using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using tnats_mobile.ViewModels;
using tnats_mobile.Views;
using Xamarin.Forms;

namespace tnats_mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            //await Shell.Current.GoToAsync("//ItemsPage");


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

            //Shell.SetFlyoutBehavior(, FlyoutBehavior.Flyout);
            await Navigation.PushAsync(new NewItemPage(file, GetPhoto(file.Path)));

        }

        public static byte[] GetPhoto(string filePath)
        {
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte[] photo = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return photo;
        }

        private async void MenuItem_Clicked_2(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//AboutPage");
        }
    }
}
