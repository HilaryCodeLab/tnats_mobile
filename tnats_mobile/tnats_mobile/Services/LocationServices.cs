using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace tnats_mobile.Services
{
    public class LocationServices
    {
        public static async Task<Location> GetLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    return location;
                    //Debug.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Debug.WriteLine(fnsEx.Message);
                Debug.WriteLine(fnsEx.StackTrace);
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Debug.WriteLine(fneEx.Message);
                Debug.WriteLine(fneEx.StackTrace);
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                Debug.WriteLine(pEx.Message);
                Debug.WriteLine(pEx.StackTrace);
                // Handle permission exception
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
                // Unable to get location
            }

            return null;
        }         
    }
}
