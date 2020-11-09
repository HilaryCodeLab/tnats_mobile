using Xamarin.Forms;
using tnats_mobile.Services;
using tnats_mobile.Views;
using System.Threading.Tasks;

namespace tnats_mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new HomePage());
            //MainPage = new HomePage();
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
