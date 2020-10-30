using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using tnats_mobile.Models;
using tnats_mobile.ViewModels;
using Plugin.Media.Abstractions;
using System.Collections.ObjectModel;
using System.Linq;
using tnats_mobile.Services;

namespace tnats_mobile.Views
{
    public partial class NewItemPage : ContentPage
    {
        ObservableCollection<string> data = new ObservableCollection<string>();
        public byte[] photo { get; set; }
        public NewItemPage()
        {
            InitializeComponent();
            //BindingContext = new NewItemViewModel(null);
            ListOfStore();
        }

        public NewItemPage(MediaFile file, byte[] p)
        {
            photo = p;
            InitializeComponent();
            //BindingContext = new NewItemViewModel(p);
            ListOfStore();

            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        public async void ListOfStore()
        {
            try
            {
                data.Add("Austria");
                data.Add("Australia");
                data.Add("Azerbaijan");
                data.Add("Bahrain");
                data.Add("Bangladesh");
                data.Add("Belgium");
                data.Add("Botswana");
                data.Add("China");
                data.Add("Colombia");
                data.Add("Denmark");
                data.Add("Kmart");
                data.Add("Pakistan");
            }
            catch (Exception ex)
            {
                await DisplayAlert("", "" + ex, "Ok");
            }
        }

        private void speciesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            String listsd = e.Item as string;
            txtLocation.Text = listsd;
            ShowLocationList(false);

            ((ListView)sender).SelectedItem = null;
        }

        private void txtLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowLocationList(true);
            locationListView.BeginRefresh();

            try
            {
                var dataEmpty = data.Where(i => i.ToLower().Contains(e.NewTextValue.ToLower()));

                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                    ShowLocationList(false);
                else if (dataEmpty.Count() == 0)
                    ShowLocationList(false);
                else
                    locationListView.ItemsSource = data.Where(i => i.ToLower().Contains(e.NewTextValue.ToLower()));
            }
            catch (Exception ex)
            {
                ShowLocationList(false);
            }
            locationListView.EndRefresh();
        }

        private void pSpecies_Focused(object sender, FocusEventArgs e)
        {
            pSpecies.ItemsSource = data;
        }

        private void ShowLocationList(bool bVisible)
        {
            lblSpecies.IsVisible = pSpecies.IsVisible = lblNotes.IsVisible = txtNotes.IsVisible = stackButtons.IsVisible = !bVisible;
            locationListView.IsVisible = bVisible;
        } 

        private async void btnCancel_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void btnSave_Clicked(object sender, EventArgs e)
        {
            var location = await LocationServices.GetLocation();

            double? longitude = null;
            double? latitude = null;

            if (location != null)
            {
                longitude = location.Longitude;
                latitude = location.Latitude;
            }

            Observation newObs = new Observation()
            {
                guid = Guid.NewGuid(),
                user_id = 1,
                location = txtLocation.Text,
                species = pSpecies.SelectedItem.ToString(),
                notes = txtNotes.Text,
                photo = photo,
                approved = false,
                active = true,
                longitude = longitude,
                latitude = latitude
            };

            await App.Database.SaveItemAsync(newObs);


            bool b = new ApiServices().test3(newObs);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}