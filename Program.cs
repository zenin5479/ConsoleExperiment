using System;
using System.Collections.Generic;
using System.Net;

namespace ConsoleExperiment
{
   internal class Program
   {
      static void Main()
      {
         WebClient user = new WebClient();
         string uri = "https://www.mheducation.com";
         string fname = "data.txt";

         try
         {
            Console.WriteLine("Загрузка данных по адресу " +
                              uri + " в файл " + fname);
            user.DownloadFile(uri, fname);
         }
         catch (WebException exc)
         {
            Console.WriteLine(exc);
         }
         Console.WriteLine("Загрузка завершена.");

         Console.ReadKey();
      }
   }
}