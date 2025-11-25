using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;

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

         // Наиболее простым, способом инициализации GET и POST запросов,
         // является использование объекта класса WebClient из пространства имён System.Net
         // Рассмотрим реализацию GET запроса
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