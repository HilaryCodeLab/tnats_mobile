using System;
using Xamarin.Forms;

using tnats_mobile.Models;
using Plugin.Media.Abstractions;
using System.Collections.ObjectModel;
using System.Linq;
using tnats_mobile.Services;
using System.Threading.Tasks;
using System.IO;

namespace tnats_mobile.Views
{
    public partial class NewItemPage : ContentPage
    {
        ObservableCollection<string> speciesList = new ObservableCollection<string>();
        ObservableCollection<string> locationsList = new ObservableCollection<string>();
        public byte[] photo { get; set; }
        public string token { get; set; }

        /// <summary>
        /// CONSTRUCTOR CLASS
        /// </summary>
        /// <param name="file">PHOTO </param>
        /// <param name="p"></param>
        /// <param name="t"></param>
        public NewItemPage(MediaFile file, string t)
        {
            InitializeComponent();

            photo = GetPhoto(file.Path);
            token = t;
            ListOfStore();

            PhotoImage.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            });
        }

        /// <summary>
        /// METHOD THAT LOADS THE SPECIES AND LOCATION LOCAL VARIABLES
        /// </summary>
        public async void ListOfStore()
        {
            try
            {
                var species = await App.Database.GetSpecies();
                foreach (var item in species.OrderBy(x => x.species))
                {
                    speciesList.Add(item.species);
                }

                var locations = await App.Database.GetLocations();
                foreach (var item in locations.OrderBy(x => x.location))
                {
                    locationsList.Add(item.location);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("", "" + ex, "Ok");
            }
        }

        /// <summary>
        /// LOCATION LIST VIEW CLICK EVENT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void locationListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            String listsd = e.Item as string;
            txtLocation.Text = listsd;
            ShowLocationList(false);

            ((ListView)sender).SelectedItem = null;
        }

        /// <summary>
        /// LOCATION TEXT BOX CHANGE EVENT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// SPECIES CONTROL ON FOCUS EVENT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pSpecies_Focused(object sender, FocusEventArgs e)
        {
            pSpecies.ItemsSource = speciesList;
        }

        /// <summary>
        /// METHOD THAT SET THE CONTROLS VISIBILITY
        /// </summary>
        /// <param name="bVisible"></param>
        private void ShowLocationList(bool bVisible)
        {
            lblSpecies.IsVisible = pSpecies.IsVisible = lblNotes.IsVisible = txtNotes.IsVisible = stackButtons.IsVisible = !bVisible;
            locationListView.IsVisible = bVisible;
        }

        /// <summary>
        /// "CANCEL" CLICK EVENT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = new HomePage();
        }

        /// <summary>
        /// "SAVE" CLICK EVENT
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            if (DependencyService.Get<INetworkAvailable>().IsNetworkAvailable())
                await Task.Run(() => new ApiServices().SaveObservation(newObs, token));

            await DisplayAlert("Observation", "Addded successfully!", "OK");

            Application.Current.MainPage = new HomePage();
        }

        /// <summary>
        /// METHOD THAT RETURNS A BYTE ARRAY OF THE PHOTO
        /// </summary>
        /// <param name="filePath">PATH OF THE PHOTO</param>
        /// <returns>ARRAY OF BYTES</returns>
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