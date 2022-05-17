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

        public static async Task<string> UploadFile(string filePath, string token, string fileName, string observedPath)
        {
            byte[] fileContent;

            while (true)
            {
                try
                {
                    fileContent = File.ReadAllBytes(filePath);
                    break;
                }
                catch
                {
                    Thread.Sleep(100);
                }
            }

            string relativePath = filePath.Replace(observedPath, "");
            using (HttpClient client = new())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using var content = new MultipartFormDataContent();
                
                content.Add(new StreamContent(new MemoryStream(fileContent)), "file", fileName);

                using var res = await client.PostAsync(baseURL + "File/uploadFileByFileWatcher?relativePath=" + relativePath, content);

                content.Dispose();

                using HttpContent responseContent = res.Content;
                string data = await responseContent.ReadAsStringAsync();
            
                if (data != null)
                {
                    return data;
                }

                client.Dispose();
            }

            return string.Empty;
        }

        public static async Task<string> DeleteFile(string filePath, string observedPath, string token, string username)
        {
            string relativePath = filePath.Replace(observedPath, "");
            relativePath = $@"{username}{relativePath}";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (HttpResponseMessage res = await client.DeleteAsync(baseURL + "File/deleteFile?relativePath=" + relativePath))
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
    }
}
