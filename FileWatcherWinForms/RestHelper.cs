using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace FileWatcherWinForms
{
    internal class RestHelper
    {
        private static readonly string baseURL = "https://localhost:44390/api/";

        public static async Task<string> Login(User user)
        {
           
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage res = await client.PostAsJsonAsync(baseURL + "Users/login", user))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            return data;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static async Task<string> UploadFile(string filePath, string token)
        {
            string allToken = "Bearer ";
            allToken += token;
            using (HttpClient client = new HttpClient())
            {
                //client.DefaultRequestHeaders.Add("Authorization", allToken);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var formData = new MultipartFormDataContent())
                {
                    Stream fileStream = File.OpenRead(filePath);
                    HttpContent fileStreamContent = new StreamContent(fileStream);
                    //fileStreamContent.Headers.Add("Authorization", allToken);
                    formData.Add(fileStreamContent, "file", "PlikTestowy.txt");
                    //formData.Headers.Add("Authorization", allToken);
                    using (HttpResponseMessage res = await client.PostAsync(baseURL + "File/uploadFile", formData))
                    {
                        using (HttpContent content = res.Content)
                        {
                            string data = await content.ReadAsStringAsync();
                            if (data != null)
                            {
                                return data;
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
