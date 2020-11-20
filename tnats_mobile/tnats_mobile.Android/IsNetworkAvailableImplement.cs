using Java.Lang;
using tnats_mobile.Droid;
using tnats_mobile.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(IsNetworkAvailableImplement))]
namespace tnats_mobile.Droid
{
    public class IsNetworkAvailableImplement : INetworkAvailable
    {
        /// <summary>
        /// CONSTRUCTOR CLASS
        /// </summary>
        public IsNetworkAvailableImplement()
        {
        }
        /// <summary>
        /// METHOD THAT CHECK IF THERE ARE ANY INTERNET CONNECTION AVAILABLE
        /// </summary>
        /// <returns>TRUE IF THERE IS INTERNET OR FALSE IF NOT</returns>
        public bool IsNetworkAvailable()
        {
            Runtime runtime = Runtime.GetRuntime();

            Process process = runtime.Exec("ping -c 3 www.google.com");

            return (process.WaitFor() == 0);
        }
    }
}