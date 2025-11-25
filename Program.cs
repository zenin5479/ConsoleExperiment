using System;
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
         string contents;
         // Адрес ресурса, к которому выполняется запрос
         string url = "https://example.com";
         // Создаём объект WebClient
         using (WebClient client = new WebClient())
         {
            client.QueryString.Add("format", "json");
            // Выполняем запрос по адресу и получаем ответ в виде строки
            contents = client.DownloadString(url);
         }

         Console.WriteLine(contents);

         // POST запрос реализует не сложнее, но уже предполагает обязательное наличие параметров запроса 
         //string ur = "https://example.com";
         //using (var webClient = new WebClient())
         //{
         //   webClient.QueryString.Add("format", "json");
         //   // Выполняем запрос по адресу и получаем ответ в виде строки
         //   contents = webClient.UploadString(ur, contents);
         //   Console.WriteLine(contents);
         //}

         // Создание WebClient
         using (WebClient client = new WebClient())
         {
            // Добавление заголовков (опционально)
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            // Выполнение GET-запроса и получение ответа в виде строки
            string response = client.DownloadString("https://example.com");

            // Далее можно работать с response, например, десериализовать JSON
         }

         // Данные для отправки
         var userData = new { Name = "John", Age = 30 };
         string jsonData = JsonConvert.SerializeObject(userData);

         using (WebClient client = new WebClient())
         {
            // Установка заголовка Content-Type
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            // Отправка POST-запроса с JSON в теле
            string response = client.UploadString("https://example.com", "POST", jsonData);

            Console.WriteLine(response);
         }

         Console.ReadKey();
      }
   }
}