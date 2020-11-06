using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using tnats_mobile.Models;

namespace tnats_mobile.Services
{
    public class RestService
    {
        HttpClient client;
        string grant_type = "password";

        public RestService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded'"));

        }

        public async Task<Token> Login(User user)
        {
            var postData = new List<KeyValuePair<string,string>>();
            postData.Add(new KeyValuePair<string, string>("grant_type", grant_type));
            postData.Add(new KeyValuePair<string, string>("username", user.Username));
            postData.Add(new KeyValuePair<string, string>("password", user.Password));
            var content = new FormUrlEncodedContent(postData);
            var weburl = Util.Constants.RestUrl;
            var response = await PostResponseLogin<Token>(weburl, content);
            DateTime dt = new DateTime();
            dt = DateTime.Today;
            response.Expire_date = dt.AddSeconds(response.Expire_in);
            return response;
        }

        public async Task<T> PostResponseLogin<T>( string weburl, FormUrlEncodedContent content) where T:class
        {
            var response = await client.PostAsync(weburl, content);
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            var responseObject= JsonConvert.DeserializeObject<T>(jsonResult);
            return responseObject;
        }

        public async Task<T> PostResponse<T>(string weburl, string jsonstring) where T:class
        {
            var Token = App.TokenDatabase.GetToken();
            string ContentType = "application/json";
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Access_token);
            try
            {
                var Result = await client.PostAsync(weburl, new StringContent(jsonstring, Encoding.UTF8, ContentType));
                if (Result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JsonResult = Result.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var ContentResp = JsonConvert.DeserializeObject<T>(JsonResult);
                        return ContentResp;
                    }
                    catch
                    {
                        return null; 
                    }
                }
                
            }
            catch
            {
                return null;
            }
            return null;
        }

        public async Task<T> GetResponse<T>(string weburl) where T : class
        {
            var Token = App.TokenDatabase.GetToken();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token.Access_token);
            try
            {
                var response = await client.GetAsync(weburl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResult = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var contentResp = JsonConvert.DeserializeObject<T>(jsonResult);
                        return contentResp;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
            catch { return null; }
            return null;
            
        }
    }
}
