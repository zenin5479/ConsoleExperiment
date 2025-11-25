using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;

// Отправка синхронных запросов с помощью HttpClient

namespace ConsoleExperiment
{
   internal class Program
   {
      static void Main()
      {
         HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://example.com");
         request.Method = "GET";
         using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
         {
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
            {
               string result = streamReader.ReadToEnd();
               Console.WriteLine(result);
            }
         }

         string contents;
         string url = "https://example.com";

         using (WebClient client = new WebClient())
         {
            contents = client.DownloadString(url);
         }

         Console.WriteLine(contents);


         var httpClient = new HttpClient();
         using (var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:12345/Coder"))
         {
            var json = JsonSerializer.Serialize(new Coder() { Name = "Bob", Language = "C#" });
            request.Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var response = httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            using var streamReader = new StreamReader(response.Content.ReadAsStream());
            Console.WriteLine(streamReader.ReadToEnd());
         }


         Console.ReadKey();
      }
   }
}