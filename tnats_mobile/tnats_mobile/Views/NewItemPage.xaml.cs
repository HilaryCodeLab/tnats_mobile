using System;
using Xamarin.Forms;

using tnats_mobile.Models;
using Plugin.Media.Abstractions;
using System.Collections.ObjectModel;
using System.Linq;
using tnats_mobile.Services;
using System.Threading.Tasks;

namespace tnats_mobile.Views
{
    public partial class NewItemPage : ContentPage
    {
        ObservableCollection<string> speciesList = new ObservableCollection<string>();
        ObservableCollection<string> locationsList = new ObservableCollection<string>();
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
                var species = await App.Database.GetSpecies();
                foreach (var item in species)
                {
                    speciesList.Add(item.species);
                }

                var locations = await App.Database.GetLocations();
                foreach (var item in locations)
                {
                    locationsList.Add(item.location);
                }
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
                var dataEmpty = locationsList.Where(i => i.ToLower().Contains(e.NewTextValue.ToLower()));

                if (string.IsNullOrWhiteSpace(e.NewTextValue))
                    ShowLocationList(false);
                else if (dataEmpty.Count() == 0)
                    ShowLocationList(false);
                else
                    locationListView.ItemsSource = locationsList.Where(i => i.ToLower().Contains(e.NewTextValue.ToLower()));
            }
            catch (Exception ex)
            {
                ShowLocationList(false);
            }
            locationListView.EndRefresh();
        }

        private void pSpecies_Focused(object sender, FocusEventArgs e)
        {
            pSpecies.ItemsSource = speciesList;
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

            await Task.Run(() => new ApiServices().SaveObservation(newObs));

            // This will pop the current page off the navigation stack
            //  await Shell.Current.GoToAsync("..");

            await Navigation.PopAsync();
        }
    }
}