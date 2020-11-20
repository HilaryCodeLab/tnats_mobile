using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Linq;
using tnats_mobile.Models;
using tnats_mobile.Util;
using Xamarin.Forms;

namespace tnats_mobile.Services
{
    public class ApiServices
    {
        /// <summary>
        /// METHOD THAT CALLS AN API TO RETRIEVE THE TOKEN USED TO USE OTHER API
        /// </summary>
        /// <param name="pEmail">USERNAME</param>
        /// <param name="pPassword">PASSWORD</param>
        /// <returns></returns>
        public string Login(string pEmail, string pPassword)
        {
            DependencyService.Get<ILogClass>().AddInfo("Login", "Begin");

            var apiInput = new { email = pEmail, password = pPassword };

            var client = new RestClient();
            var request = new RestRequest(Constants.RestUrl + "/api/login", Method.POST, DataFormat.Json);

            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(apiInput);

            string token = "";
            try
            {
                IRestResponse response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    JObject jObject = JObject.Parse(response.Content);

                    token = jObject["token"].ToString();

                    App.Database.DeleteUser();
                    App.Database.SaveUser(new User { Username = pEmail, Password = pPassword, Token = token });

                    DependencyService.Get<ILogClass>().AddInfo("Login", "Successful");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogClass>().AddError("Login", ex.Message);
                DependencyService.Get<ILogClass>().AddError("Login", ex.StackTrace);
            }

            return token;
        }

        /// <summary>
        /// METHOD THAT CALLS AN API TO SAVE THE OBSERVATION TO THE CLOUD DATABASE
        /// </summary>
        /// <param name="obs">OBSERVATION OBJECT</param>
        /// <param name="token">USER TOKEN TO ACCESS THE API</param>
        public async void SaveObservation(Observation obs, string token)
        {
            DependencyService.Get<ILogClass>().AddInfo("SaveObservation", "Begin");

            var photo_string = Convert.ToBase64String(obs.photo);
            obs.photo = null;

            var client = new RestClient();
            var request = new RestRequest(Constants.RestUrl + "/api/addObs", Method.POST, DataFormat.Json);

            request.AddJsonBody(obs);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            try
            {
                IRestResponse response = await client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    DependencyService.Get<ILogClass>().AddInfo("SaveObservation", "Expired Token");

                    var user = await App.Database.GetLoggedUser();
                    token = Login(user.Username, user.Password);
                    SaveObservation(obs, token);
                }
                else
                {
                    if (response.IsSuccessful)
                    {
                        DependencyService.Get<ILogClass>().AddInfo("SaveObservation", "Successful");
                        TransferPhotoString(obs, photo_string, token);
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogClass>().AddError("SaveObservation", ex.Message);
                DependencyService.Get<ILogClass>().AddError("SaveObservation", ex.StackTrace);
            }
        }

        /// <summary>
        /// METHOD THAT CALLS AN API TO SAVE THE PHOTO TO THE CLOUD DATABASE
        /// </summary>
        /// <param name="obs">OBSERVATION OBJECT</param>
        /// <param name="photo_string">PHOTO CONVERTED AS BASE 64 STRING</param>
        /// <param name="token">USER TOKEN TO ACCESS THE API</param>
        /// <param name="size">NUMBER OF CHAR TO BE SENT</param>
        public async void TransferPhotoString(Observation obs, string photo_string, string token, int size = 0)
        {
            DependencyService.Get<ILogClass>().AddInfo("TransferPhotoString", "Begin");

            if (size == 0)
            {
                size = photo_string.Length / 10;
            }
            else if (size > photo_string.Length)
            {
                size = photo_string.Length;
            }

            string ps = photo_string.Substring(0, size);

            photo_string = photo_string.Replace(ps, "");

            var apiInput = new
            {
                guid = obs.guid,
                photo_string = ps,
                end_of_photo = string.IsNullOrEmpty(photo_string)
            };

            var client = new RestClient();
            var request = new RestRequest(Constants.RestUrl + "/api/addObs", Method.POST, DataFormat.Json);

            request.AddJsonBody(apiInput);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            try
            {
                IRestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(photo_string))
                        TransferPhotoString(obs, photo_string, token, size);
                    else
                    {
                        await App.Database.DeleteItemAsync(obs);
                        DependencyService.Get<ILogClass>().AddInfo("TransferPhotoString", "Successful");
                    }
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogClass>().AddError("TransferPhotoString", ex.Message);
                DependencyService.Get<ILogClass>().AddError("TransferPhotoString", ex.StackTrace);
            }
        }

        /// <summary>
        /// METHOD THAT CALLS AN API TO RETRIVE A LIST OF SPECIES
        /// </summary>
        /// <param name="token">USER TOKEN TO ACCESS THE API</param>
        public async void GetSpecies(string token)
        {
            DependencyService.Get<ILogClass>().AddInfo("GetSpecies", "Begin");

            var client = new RestClient();
            var request = new RestRequest(Constants.RestUrl + "/api/getSpecies", Method.GET, DataFormat.Json);

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            try
            {
                IRestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    App.Database.DeleteAllSpecies();

                    JObject jObject = JObject.Parse(response.Content);

                    for (int i = 1; i < jObject["species"].Count(); i++)
                    {
                        await App.Database.SaveSpecies(new Species { species = jObject["species"][i]["Species"].ToString() });
                    }

                    DependencyService.Get<ILogClass>().AddInfo("GetSpecies", "Successful");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogClass>().AddError("GetSpecies", ex.Message);
                DependencyService.Get<ILogClass>().AddError("GetSpecies", ex.StackTrace);
            }
        }

        /// <summary>
        /// METHOD THAT CALLS AN API TO RETRIVE A LIST OF LOCATIONS
        /// </summary>
        /// <param name="token">USER TOKEN TO ACCESS THE API</param>
        public async void GetLocations(string token)
        {
            DependencyService.Get<ILogClass>().AddInfo("GetLocations", "Begin");

            var client = new RestClient();
            var request = new RestRequest(Constants.RestUrl + "/api/getLocations", Method.GET, DataFormat.Json);

            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            try
            {
                IRestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    App.Database.DeleteAllLocations();

                    JObject jObject = JObject.Parse(response.Content);

                    for (int i = 0; i < jObject["locations"].Count(); i++)
                    {
                        await App.Database.SaveLocation(new Location { location = jObject["locations"][i]["Location"].ToString() });
                    }

                    DependencyService.Get<ILogClass>().AddInfo("GetLocations", "Successful");
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<ILogClass>().AddError("GetLocations", ex.Message);
                DependencyService.Get<ILogClass>().AddError("GetLocations", ex.StackTrace);
            }
        }
    }
}
