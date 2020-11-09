﻿using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tnats_mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                var user = await App.Database.GetLoggedUser();

                if (user == null)
                { 
                    await Navigation.PushAsync(new LoginPage());
                }
             });
        }
        async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            App.Database.DeleteUser();
            //await Task.Run(() => { App.Database.DeleteUser(); });
            //Navigation.InsertPageBefore(new LoginPage(), this);
            //await Navigation.PopAsync();
            //await Navigation.PushModalAsync(new LoginPage());
            await Navigation.PushAsync(new LoginPage());
        }

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
    }
}