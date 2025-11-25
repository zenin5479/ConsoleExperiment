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
         // Адрес ресурса, к которому выполняется запрос
         string url = "https://example.com";
         // Создаём объект WebClient
         using (WebClient client = new WebClient())
         {
            // Выполняем запрос по адресу и получаем ответ в виде строки
            contents = client.DownloadString(url);
         }

         Console.WriteLine(contents);


         string url = "http://site.com/";


         using (var webClient = new WebClient())
         {

            var response = webClient.DownloadString(url);
         }

         Console.ReadKey();
      }
   }
}