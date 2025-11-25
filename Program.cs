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
         var request = (HttpWebRequest)WebRequest.Create("https://example.com");
         request.Method = "GET";
         using (var response = (HttpWebResponse)request.GetResponse())
         using (var streamReader = new StreamReader(response.GetResponseStream()))
         {
            var result = streamReader.ReadToEnd();
            Console.WriteLine(result);
         }





         Console.ReadKey();
      }
   }
}