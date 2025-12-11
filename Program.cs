using System;
using System.IO;
using System.Net;

namespace ConsoleExperiment
{
   internal class Program
   {
      static void Main()
      {
         int ch;
         // Сначала создать объект запроса типа WebRequest по указанному URI.
         HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.mheducation.com");
         // Затем отправить сформированный запрос и получить на него ответ.
         HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
         // Получить из ответа поток ввода.
         Stream istrm = resp.GetResponseStream();
         /* А теперь прочитать и отобразить гипертекстовое содержимое,
             полученное по указанному URI. Это содержимое выводится на экран
             отдельными порциями по 400 символов. После каждой такой порции
             следует нажать клавишу <ENTER>, чтобы вывести на экран
             следующую порцию из 400 символов. */
         for (int i = 1; ; i++)
         {
            ch = istrm.ReadByte();
            if (ch == -1) break;
            Console.Write((char)ch);
            if ((i % 400) == 0)
            {
               Console.Write("\nНажмите клавишу <Enter>.");
               Console.ReadLine();
            }
         }

         // Закрыть ответный поток. При этом закрывается также поток ввода istrm.
         resp.Close();


         // Применение класса WebClient
         WebClient user = new WebClient();
         string uri = "https://www.mheducation.com";
         string fname = "data.txt";
         try
         {
            Console.WriteLine("Загрузка данных по адресу " + uri + " в файл " + fname);
            user.DownloadFile(uri, fname);
         }
         catch (WebException exc)
         {
            Console.WriteLine(exc);
         }

         Console.WriteLine("Загрузка завершена");

         Console.ReadKey();
      }
   }
}