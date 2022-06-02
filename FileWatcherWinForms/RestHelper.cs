using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

        public static async Task<string> GetUserFile(string token, string username)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (HttpResponseMessage res = await client.GetAsync(baseURL + "File/getUserFiles"))
                {
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                       
                        if (data != null)
                        {
                            //var files = new List<FileDTO>();
                            return data;
                        }
                    }
                }
            }
            return string.Empty;
        }

        public static async Task<string> DownloadFile(string token, Guid idFile, string fileName)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                using (var res = await client.GetAsync(baseURL + "File/downloadFile?fileId=" +idFile))
                {
                    using (HttpContent content = res.Content)
                    {
                        var data = await content.ReadAsStreamAsync();

                        if (data != null)
                        {
                            var fileInfo = new FileInfo(fileName);
                            using (var fileStream = fileInfo.OpenWrite())
                            {
                                await data.CopyToAsync(fileStream);
                            }
                            return res.StatusCode.ToString();
                            //return File(data, content.GetType, content.Headers);
                        }
                    }
                }
            }
            return string.Empty;
        }

    }
}
