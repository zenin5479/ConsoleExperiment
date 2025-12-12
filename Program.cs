using System;
using System.IO;
using System.Net;

namespace ConsoleExperiment
{
   internal class Program
   {
      static void Main()
      {
         ExampleAccessingWebsite();
         ExampleAccessingSite();
         HandlingNetworkException();
         ApplyingPropertiesUriClass();

         UsingWebClientClass();

         Console.ReadKey();
      }

      // Доступ к заголовку с информацией
      static void AccessHeaderInformation()
      {



      }

      // Пример применения свойств из класса Uri
      static void ApplyingPropertiesUriClass()
      {
         Uri sample = new Uri("https://example.com/somefile.txt?SomeQuery");
         Console.WriteLine("Хост: " + sample.Host);
         Console.WriteLine("Порт: " + sample.Port);
         Console.WriteLine("Протокол: " + sample.Scheme);
         Console.WriteLine("Локальный путь: " + sample.LocalPath);
         Console.WriteLine("Запрос: " + sample.Query);
         Console.WriteLine("Путь и запрос: " + sample.PathAndQuery);
      }

      // Пример обработки сетевых исключений
      static void HandlingNetworkException()
      {
         int ch = 0;
         try
         {
            // Сначала создать объект запроса типа WebRequest по указанному URI
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://example.com");
            // Затем отправить сформированный запрос и получить на него ответ
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            // Получить из ответа поток ввода
            Stream istrm = resp.GetResponseStream();
            // А теперь прочитать и отобразить гипертекстовое содержимое, полученное по указанному URI
            // Это содержимое выводится на экран отдельными порциями по 400 символов
            // После каждой такой порции следует нажать клавишу <ENTER>, чтобы вывести на экран следующую порцию из 400 символов
            for (int i = 1; ; i++)
            {
               if (istrm != null) ch = istrm.ReadByte();
               if (ch == -1)
               {
                  break;
               }

               Console.Write((char)ch);
               if (i % 400 == 0)
               {
                  Console.Write("\nНажмите клавишу <Enter>");
                  Console.ReadLine();
               }
            }

            // Закрыть ответный поток
            // При этом закрывается также поток ввода istrm
            resp.Close();
         }
         catch (WebException exc)
         {
            Console.WriteLine("Сетевая ошибка: " + exc.Message + "\nКод состояния: " + exc.Status);
         }
         catch (ProtocolViolationException exc)
         {
            Console.WriteLine("Протокольная ошибка: " + exc.Message);
         }
         catch (UriFormatException exc)
         {
            Console.WriteLine("Ошибка формата URI: " + exc.Message);
         }
         catch (NotSupportedException exc)
         {
            Console.WriteLine("Неизвестный протокол: " + exc.Message);
         }
         catch (IOException exc)
         {
            Console.WriteLine("Ошибка ввода-вывода: " + exc.Message);
         }
         catch (System.Security.SecurityException exc)
         {
            Console.WriteLine("Исключение в связи с нарушением безопасности: " + exc.Message);
         }
         catch (InvalidOperationException exc)
         {
            Console.WriteLine("Недопустимая операция: " + exc.Message);
         }
      }

      // Пример доступа к сайту на основе классов WebRequest и WebResponse
      static void ExampleAccessingSite()
      {
         int ch = 0;
         // Сначала создать объект запроса типа WebRequest по указанному URI
         WebRequest req = WebRequest.Create("https://example.com");
         // Затем отправить сформированный запрос и получить на него ответ
         WebResponse resp = req.GetResponse();
         // Получить из ответа поток ввода
         Stream istrm = resp.GetResponseStream();
         // А теперь прочитать и отобразить гипертекстовое содержимое, полученное по указанному URI
         // Это содержимое выводится на экран отдельными порциями по 400 символов
         // После каждой такой порции следует нажать клавишу <ENTER>, чтобы вывести на экран следующую порцию из 400 символов
         for (int i = 1; ; i++)
         {
            if (istrm != null)
            {
               ch = istrm.ReadByte();
            }

            if (ch == -1)
            {
               break;
            }

            Console.Write((char)ch);
            if ((i % 400) == 0)
            {
               Console.Write("\nНажмите клавишу <Enter>");
               Console.ReadLine();
            }
         }

         // Закрыть ответный поток
         // При этом закрывается также поток ввода istrm
         resp.Close();
      }

      // Пример доступа к сайту на основе классов WebRequest и WebResponse
      static void ExampleAccessingWebsite()
      {
         int ch = 0;
         // Сначала создается объект запроса типа WebRequest по указанному URI
         HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://example.com");
         // Затем отправить сформированный запрос и получить на него ответ
         HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
         // Получить из ответа поток ввода
         Stream istrm = resp.GetResponseStream();
         // А теперь прочитать и отобразить гипертекстовое содержимое, полученное по указанному URI
         // Это содержимое выводится на экран отдельными порциями по 400 символов
         // После каждой такой порции следует нажать клавишу <ENTER>, чтобы вывести на экран следующую порцию из 400 символов
         for (int i = 1; ; i++)
         {
            if (istrm != null)
            {
               ch = istrm.ReadByte();
            }

            if (ch == -1)
            {
               break;
            }

            Console.Write((char)ch);
            if ((i % 400) == 0)
            {
               Console.Write("\nНажмите клавишу <Enter>");
               Console.ReadLine();
            }
         }

         // Закрыть ответный поток
         // При этом закрывается также поток ввода istrm
         resp.Close();
      }

      // Применение класса WebClient
      static void UsingWebClientClass()
      {
         // Применение класса WebClient
         WebClient user = new WebClient();
         string uri = "https://example.com";
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
      }
   }
}