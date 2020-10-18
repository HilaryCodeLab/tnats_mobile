﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using tnats_mobile.Services;
using tnats_mobile.Views;

namespace tnats_mobile
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        static ObservationDatabase database;
        public static ObservationDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ObservationDatabase();
                }
                return database;
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
