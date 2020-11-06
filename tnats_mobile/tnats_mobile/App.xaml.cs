using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using tnats_mobile.Services;
using tnats_mobile.Views;
using tnats_mobile.Data;
using tnats_mobile.Services;


namespace tnats_mobile
{
    public partial class App : Application
    {
        public static string AppName { get { return "StoreAccountInfoApp"; } }

        public static ICredentialsService CredentialsService { get; private set; }

        public App()
        {
            //InitializeComponent();
            //DependencyService.Register<MockDataStore>();
            ////MainPage = new AppShell();
            //MainPage = new LoginPage();
            CredentialsService = new CredentialsService();
            if (CredentialsService.DoCredentialsExist())
            {
                MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                MainPage = new NavigationPage(new LoginPage());
            }

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

        static UserDatabaseController userDatabase;
        public static UserDatabaseController UserDatabase
        {
            get
            {
                if (userDatabase == null)
                {
                    userDatabase = new UserDatabaseController();
                }
                return userDatabase;
            }
        }

        static ApiServices apiService;
        public static ApiServices ApiService
        {
            get
            {
                if (apiService == null)
                {
                    apiService = new ApiServices();
                }
                return apiService;
            }

        }

        static TokenDatabaseController tokenDatabase;
        public static TokenDatabaseController TokenDatabase
        {
            get
            {
                if (tokenDatabase == null)
                {
                    tokenDatabase = new TokenDatabaseController();
                }
                return tokenDatabase;
            }
        }

        static RestService restService;
        public static RestService RestService
        {
            get
            {
                if (restService == null)
                {
                    restService = new RestService();
                }
                return restService;
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
