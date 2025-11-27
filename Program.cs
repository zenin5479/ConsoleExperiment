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
         // Наиболее простым, способом инициализации GET и POST запросов,
         // является использование объекта класса WebClient из пространства имён System.Net
         // Реализация GET запроса

         string contents;
         // Адрес ресурса, к которому выполняется запрос
         string url = "https://example.com";
         // Создаём объект WebClient
         using (WebClient client = new WebClient())
         {
            // Выполняем запрос по адресу и получаем ответ в виде строки
            contents = client.DownloadString(url);
            Console.WriteLine("GET Response: " + contents);
         }

         Console.WriteLine();

         // POST - запрос
         using (WebClient customer = new WebClient())
         {
            // Данные для отправки
            NameValueCollection data = new NameValueCollection
            {
               { "key1", "value1" },
               { "key2", "value2" }
            };

            // Выполняем POST-запрос
            byte[] responseBytes = customer.UploadValues("https://httpbin.org/post", "POST", data);
            string response = System.Text.Encoding.UTF8.GetString(responseBytes);
            Console.WriteLine("POST Response: " + response);
         }

         WebClient user = new WebClient();
         string address = "https://vk.com";
         string fileName = "Data.txt";
         try
         {
            Console.WriteLine("Загрузка данных по адресу " + address + " в файл " + fileName);
            user.DownloadFile(address, fileName);
         }
         catch (WebException exc)
         {
            Console.WriteLine(exc);
         }

         Console.WriteLine("Загрузка завершена");

         Console.WriteLine("Загрузка...\nПожалуйста, подождите...");
         // Для загрузки файла используется класс WebClient
         WebClient subject = new WebClient();
         // Метод DownloadFile() принимает два параметра - первый это путь к файлу,
         // который нужно скачать, а второй - локальное имя файла
         subject.DownloadFile("https://www.google.ru", "WebClienUploadingFiles.txt");
         Console.WriteLine("Загрузка завершена");
         // Получить вебстраницу в строку для последующей ее обработки
         // Cама процедура неэффективная - сначала загружаем страницу в файл, потом читаем текст из этого файла
         // Гораздо удобнее использовать метод OpenRead()
         Stream str = subject.OpenRead("https://www.google.ru");
         // Содержимое страницы будет загружено в переменную
         // После этого можно использовать класс StreamReader для обработки потока
         if (str != null)
         {
            StreamReader reader = new StreamReader(str);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
               Console.WriteLine(line);
            }
         }

         Console.ReadKey();
      }
   }
}