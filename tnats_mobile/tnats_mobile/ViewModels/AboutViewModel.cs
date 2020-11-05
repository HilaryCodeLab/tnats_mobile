﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using tnats_mobile.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace tnats_mobile.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(() =>
           {

               bool isAvailable = DependencyService.Get<INetworkAvailable>().IsNetworkAvailable();

               if (isAvailable)
               {
                   Debug.WriteLine("network is available");
               }

               else
               {
                   Debug.WriteLine("network is unavailable");
               }
               //await Browser.OpenAsync("https://aka.ms/xamain-quickstart");


           });

            TestCommand = new Command(async () =>
            {


                //var location = await LocationServices.GetLocation();

                //if (location != null)
                //{
                //    Debug.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                //}

                //bool b = new ApiServices().test3("", "");
                var list = await new ApiServices().GetSpecies();

                
            });
        }

        public ICommand OpenWebCommand { get; }
        public ICommand TestCommand { get; }
    }
}