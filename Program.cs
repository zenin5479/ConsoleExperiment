using System;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleExperiment
{
   internal class Program
   {
      static void Main()
      {
         HttpListener server = new HttpListener();
         // Установка адресов прослушки
         server.Prefixes.Add("http://127.0.0.1:8888/connection/");
         server.Start(); // начинаем прослушивать входящие подключения
         Console.WriteLine("Сервер запущен. Ожидание подключений...");

         // Получаем контекст
         HttpListenerContext context = server.GetContext();
         // Получаем данные запроса
         HttpListenerRequest request = context.Request;

         Console.WriteLine($"адрес приложения: {request.LocalEndPoint}");
         Console.WriteLine($"адрес клиента: {request.RemoteEndPoint}");
         Console.WriteLine(request.RawUrl);
         Console.WriteLine($"Запрошен адрес: {request.Url}");
         Console.WriteLine("Заголовки запроса:");
         for (int i = 0; i < request.Headers.Keys.Count; i++)
         {
            string item = request.Headers.Keys[i];
            Console.WriteLine($"{item}:{request.Headers[item]}");
         }

         // Получаем объект для установки ответа
         HttpListenerResponse response = context.Response;
         byte[] buffer = Encoding.UTF8.GetBytes("Hello METANIT");
         // Получаем поток ответа и пишем в него ответ
         response.ContentLength64 = buffer.Length;
         using Stream output = response.OutputStream;
         // Отправляем данные
         output.Write(buffer);
         output.Flush();

         // Останавливаем сервер
         server.Stop();

         Console.ReadKey();
      }
   }
}