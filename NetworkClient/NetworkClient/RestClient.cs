using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetworkClient
{
    public class RestClient : BaseHttpClient
    {
        public RestClient(string appName) : base(appName) { }
        public RestClient(string appName, string key, string value) : base(appName, key, value) { }
        public RestClient(string appName, KeyValuePair<string, string> header) : base(appName, header.Key, header.Value) { }
        public RestClient(string appName, Dictionary<string, string> headers) : base(appName, headers) { }

        public async Task<T> GetAsync<T>(string uri)
        {
            NetworkConnection.UpdateNetworkInformation();
            if (NetworkConnection.IsConnected && NetworkConnection.IsInternetAvailable)
            {
                HttpResponseMessage response = await GetAsync(new Uri(uri, UriKind.Absolute));
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    ShowErrorDialog(response.ReasonPhrase);
                }
            }
            else
            {
                ShowMessageDialog("Please check your internet connection.");
            }

            return default(T);
        }

        public async Task<T> PostAsync<T>(string uri, object obj)
        {
            NetworkConnection.UpdateNetworkInformation();
            if (NetworkConnection.IsConnected && NetworkConnection.IsInternetAvailable)
            {
                string json = JsonConvert.SerializeObject(obj);
                StringContent postContent = new StringContent(json);
                HttpResponseMessage response = await PostAsync(new Uri(uri, UriKind.Absolute), postContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    ShowErrorDialog(response.ReasonPhrase);
                }
            }
            else
            {
                ShowMessageDialog("Please check your internet connection.");
            }

            return default(T);
        }

        public async Task<T> PutAsync<T>(string uri, object obj)
        {
            NetworkConnection.UpdateNetworkInformation();
            if (NetworkConnection.IsConnected && NetworkConnection.IsInternetAvailable)
            {
                string json = JsonConvert.SerializeObject(obj);
                StringContent putContent = new StringContent(json);
                HttpResponseMessage response = await PutAsync(new Uri(uri, UriKind.Absolute), putContent);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(content);
                }
                else
                {
                    ShowErrorDialog(response.ReasonPhrase);
                }
            }
            else
            {
                ShowMessageDialog("Please check your internet connection.");
            }

            return default(T);
        }
    }
}
