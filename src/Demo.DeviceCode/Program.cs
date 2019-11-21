using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Demo.DeviceCode
{
    class Program
    {
        static void Main(string[] args)
        {
            AsyncMain(args).GetAwaiter().GetResult();
            Console.WriteLine("Hello World!");
        }
        static async Task AsyncMain(string[] args)
        {
            string accessToken = "";
            var tokenRequest = await RequestTokenAsync<DevideFlowResponse>();
            var request = new
            {
                grant_type = "urn:ietf:params:oauth:grant-type:device_code",
                client_id = "device-flow",
                client_secret = "123456",
                device_code = tokenRequest.device_code
            };
            var requestBody = new FormUrlEncodedContent(JObject.FromObject(request).ToObject<Dictionary<string, string>>());
            while (true)
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync("https://localhost:5000/connect/token", requestBody);
                    var responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseBody);
                    var responseObject = JObject.Parse(responseBody);
                    if (responseObject["error"] != null)
                    {
                        await Task.Delay(tokenRequest.interval * 1000);
                    }
                    else
                    {

                        Console.WriteLine("AccquiredToken success");
                        accessToken = responseObject["access_token"].ToString();
                        break;
                    }
                }
            }
            // do something with access token;
            Console.WriteLine($"Do something with access token");
        }
        static async Task<T> RequestTokenAsync<T>()
        {
            string authorityEndpoint = "https://localhost:5000/connect/deviceauthorization";
            using (var httpClient = new HttpClient())
            {
                var request = new
                {
                    client_id = "device-flow",
                    client_secret = "123456",
                    scope = "openid profile course-api"
                };
                var requestBody = new FormUrlEncodedContent(JObject.FromObject(request).ToObject<Dictionary<string, string>>());
                var response = await httpClient.PostAsync(authorityEndpoint, requestBody);
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
                return JsonConvert.DeserializeObject<T>(responseString);
            }
        }
    }
    public class DevideFlowResponse
    {
        public string device_code { get; set; }
        public string verification_uri { get; set; }
        public string user_code { get; set; }
        public int expires_in { get; set; }
        public int interval { get; set; }
    }
}
