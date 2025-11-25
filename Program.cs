using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

// Отправка синхронных запросов с помощью HttpClient

namespace ConsoleExperiment
{
   internal class Program
   {
      static void Main()
      {
        // Наиболее простым, способом инициализации GET и POST запросов,
         // является использование объекта класса WebClient из пространства имён System.Net
         // Реализация GET запроса
         //string contents;
         //// Адрес ресурса, к которому выполняется запрос
         //string url = "https://example.com";
         //// Создаём объект WebClient
         //using (WebClient client = new WebClient())
         //{
         //   client.QueryString.Add("format", "json");
         //   // Выполняем запрос по адресу и получаем ответ в виде строки
         //   contents = client.DownloadString(url);
         //}

         //Console.WriteLine(contents);

         // POST запрос реализует не сложнее, но уже предполагает обязательное наличие параметров запроса 
         //string ur = "https://example.com";
         //using (var webClient = new WebClient())
         //{
         //   webClient.QueryString.Add("format", "json");
         //   // Выполняем запрос по адресу и получаем ответ в виде строки
         //   contents = webClient.UploadString(ur, webClient);
         //   Console.WriteLine(contents);
         //}


         WebClient client = new WebClient();
         // Example of posting data to a server
         string postAddress = "https://example.com";
         // Prepare string data for POST request
         string stringData = "name=John&age=30";
         byte[] postData = System.Text.Encoding.ASCII.GetBytes(stringData);
         try
         {
            byte[] response = client.UploadData(postAddress, "POST", postData);
            // Log response headers and content
            Console.WriteLine("Response received: " + System.Text.Encoding.ASCII.GetString(response));
         }
         catch (Exception ex)
         {
            Console.WriteLine("Post failed: " + ex.Message);
         }

         Console.ReadKey();
      }
   }
}