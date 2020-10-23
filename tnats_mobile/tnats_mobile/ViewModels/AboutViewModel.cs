using System;
using System.Diagnostics;
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
        }

        public ICommand OpenWebCommand { get; }
    }
}