using System;
using System.Collections.Specialized;
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
         // Вариант 1

         string contents;
         // Адрес ресурса, к которому выполняется запрос
         string url = "https://example.com";
         // Создаём объект WebClient
         using (WebClient client = new WebClient())
         {
            // Выполняем запрос по адресу и получаем ответ в виде строки
            contents = client.DownloadString(url);
         }

         Console.WriteLine(contents);
         Console.WriteLine();

         // Вариант 2
         using (WebClient client = new WebClient())
         {
            string response = client.DownloadString("https://example.com");
            Console.WriteLine(response);
         }

         using (var clients = new WebClient())
         {
            // Данные для отправки
            var data = new NameValueCollection
            {
               { "key1", "value1" },
               { "key2", "value2" }
            };

            // Выполняем POST-запрос
            byte[] responseBytes = clients.UploadValues("https://httpbin.org/post", "POST", data);
            string response = System.Text.Encoding.UTF8.GetString(responseBytes);
            Console.WriteLine("POST Response: " + response);
         }


         Console.ReadKey();
      }
   }
}