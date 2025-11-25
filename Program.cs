using System;
using System.IO;
using System.Net;

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
         string ur = "https://example.com";
         using (var webClient = new WebClient())
         {
            webClient.QueryString.Add("format", "json");
            // Выполняем запрос по адресу и получаем ответ в виде строки
            contents = webClient.DownloadString(ur);
            Console.WriteLine(contents);
         }

         Console.ReadKey();
      }
   }
}