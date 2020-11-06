using Xamarin.Forms;
using tnats_mobile.Services;
using tnats_mobile.Views;


namespace tnats_mobile
{
    public partial class App : Application
    {
        public static string AppName { get { return "StoreAccountInfoApp"; } }

        public App()
        {
            InitializeComponent();

            ////MainPage = new AppShell();
            //MainPage = new LoginPage();
            //CredentialsService = new CredentialsService();
            //if (CredentialsService.DoCredentialsExist())
            //{
            MainPage = new NavigationPage(new HomePage());
            //}
            //else
            //{
            //    MainPage = new NavigationPage(new LoginPage());
            //}

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
