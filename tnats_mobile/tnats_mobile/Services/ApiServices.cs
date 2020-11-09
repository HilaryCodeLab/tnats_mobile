using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using tnats_mobile.Models;
using tnats_mobile.Util;

namespace tnats_mobile.Services
{
    public class ApiServices
    {
        public string Login(string pEmail = "test@test.com.au2", string pPassword = "123123123")
        {
            var client = new RestClient();
            var request = new RestRequest(Constants.RestUrl + "/api/login", Method.POST, DataFormat.Json);

            var apiInput = new { email = pEmail, password = pPassword };

            request.AddJsonBody(apiInput);
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Content-Type", "application/json");

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

                    Debug.WriteLine(@"\tTodoItem successfully saved.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                Debug.WriteLine(@"\tERROR {0}", ex.StackTrace);
            }

            return token;
        }

        public async void SaveObservation(Observation obs)
        {
            var user = await App.Database.GetLoggedUser();

            string token = Login(user.Username, user.Password);

            var client = new RestClient();

            var request = new RestRequest(Constants.RestUrl + "/api/addObs", Method.POST, DataFormat.Json);

            request.AddHeader("Content-Type", "application/json");

            var photo_string = Convert.ToBase64String(obs.photo);
            obs.photo = null;

            //add parameters and token to request
            request.Parameters.Clear();
            request.AddJsonBody(obs);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            try
            {
                IRestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    TransferPhotoString(obs, photo_string, token);
                    Debug.WriteLine(@"\test successfully saved.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public async void TransferPhotoString(Observation obs, string photo_string, string token, int size = 0)
        {
            if (size == 0)
            {
                size = photo_string.Length / 10;
            }
            else if (size > photo_string.Length)
            {
                size = photo_string.Length;
            }

            string ps = photo_string.Substring(0, size);

            var client = new RestClient();

            var request = new RestRequest(Constants.RestUrl + "/api/addObs", Method.POST, DataFormat.Json);

            request.AddHeader("Content-Type", "application/json");

            ////object containing input parameter data for DoStuff() API method
            photo_string = photo_string.Replace(ps, "");

            var apiInput = new
            {
                guid = obs.guid,
                photo_string = ps,
                end_of_photo = string.IsNullOrEmpty(photo_string)
            };

            //add parameters and token to request
            request.Parameters.Clear();
            request.AddJsonBody(apiInput);
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            try
            {
                IRestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    if (!string.IsNullOrEmpty(photo_string))
                    {
                        TransferPhotoString(obs, photo_string, token, size);
                    }

                    Debug.WriteLine(@"\test successfully saved.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public async void GetSpecies()
        {
            var user = await App.Database.GetLoggedUser();

            string token = Login(user.Username, user.Password);

            var client = new RestClient();

            var request = new RestRequest(Constants.RestUrl + "/api/getSpecies", Method.GET, DataFormat.Json);

            request.AddHeader("Content-Type", "application/json");

            //add parameters and token to request
            request.Parameters.Clear();
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            try
            {
                IRestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    App.Database.DeleteAllSpecies();

                    JObject jObject = JObject.Parse(response.Content);

                    var listLen = Convert.ToInt32(jObject["species"].Last["id"].ToString());

                    for (int i = 1; i < listLen; i++)
                    {
                        await App.Database.SaveSpecies(new Species { species = jObject["species"][i]["Species"].ToString() });
                    }

                    Debug.WriteLine(@"\GetSpecies successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        public async void GetLocations()
        {
            var user = await App.Database.GetLoggedUser();

            string token = Login(user.Username, user.Password);

            var client = new RestClient();

            var request = new RestRequest(Constants.RestUrl + "/api/getLocations", Method.GET, DataFormat.Json);

            request.AddHeader("Content-Type", "application/json");

            //add parameters and token to request
            request.Parameters.Clear();
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);

            try
            {
                IRestResponse response = await client.ExecuteAsync(request);

                if (response.IsSuccessful)
                {
                    App.Database.DeleteAllLocations();

                    JObject jObject = JObject.Parse(response.Content);

                    var listLen = Convert.ToInt32(jObject["locations"].Last["id"].ToString());

                    for (int i = 1; i < listLen; i++)
                    {
                        await App.Database.SaveLocation(new Location { location = jObject["locations"][i]["Location"].ToString() });
                    }

                    Debug.WriteLine(@"\GetLocations successfully.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }
    }
}
