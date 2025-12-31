using System;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleExperiment
{
   class Program
   {
      static void Main()
      {
         CreateSynchronousServerOne();
         //CreateSynchronousServerTwo();

         Console.ReadKey();
      }

      // Метод работет без прав администратора (порт: http://127.0.0.1:8888/connection/)
      static void CreateSynchronousServerTwo()
      {
         // Создаём экземпляр HttpListener
         HttpListener listener = new HttpListener();
         // Добавляем префиксы (адреса, на которых сервер будет слушать запросы)
         // Можно указать несколько префиксов
         listener.Prefixes.Add("http://127.0.0.1:8888/connection/");
         //listener.Prefixes.Add("http://127.0.0.1:8080/");
         try
         {
            // Запускаем прослушивание
            listener.Start();
            Console.WriteLine("Сервер запущен. Ожидание запросов...");
            // Бесконечный цикл обработки запросов
            while (true)
            {
               // Блокирующий вызов: ждёт прихода запроса
               HttpListenerContext context = listener.GetContext();
               // Получаем запрос и ответ
               HttpListenerRequest request = context.Request;
               HttpListenerResponse response = context.Response;

               // Логируем запрос
               Console.WriteLine("Получен запрос: {0} {1}", request.HttpMethod, request.Url);

               // Формируем ответ
               string responseString = "<html><body><h1>Привет от синхронного сервера!</h1>" +
                                       string.Format("<p>Время: {0}</p></body></html>",
                                          DateTime.Now);
               byte[] buffer = Encoding.UTF8.GetBytes(responseString);

               // Настраиваем ответ
               response.ContentLength64 = buffer.Length;
               response.ContentType = "text/html; charset=UTF-8";
               response.StatusCode = (int)HttpStatusCode.OK;

               // Отправляем ответ клиенту
               using (Stream output = response.OutputStream)
               {
                  output.Write(buffer, 0, buffer.Length);
               }

               // Закрываем запрос (освобождаем ресурсы)
               context.Response.Close();
            }
         }
         catch (HttpListenerException ex)
         {
            Console.WriteLine("Ошибка HttpListener: {0}", ex.Message);
         }
         catch (Exception ex)
         {
            Console.WriteLine("Неожиданная ошибка: {0}", ex.Message);
         }
         finally
         {
            // Останавливаем прослушивание и освобождаем ресурсы
            listener.Stop();
            listener.Close();
            Console.WriteLine("Сервер остановлен.");
         }
      }

      // Метод работет без прав администратора (порт: http://127.0.0.1:8888/connection/)
      static void CreateSynchronousServerOne()
      {
         string port = "http://127.0.0.1:8888/connection/";
         HttpServer server = new HttpServer(port);
         Console.WriteLine("Нажмите Ctrl+C для остановки сервера...");
         void SpecifyName(object sender, ConsoleCancelEventArgs e)
         {
            e.Cancel = true;
            server.Stop();
            Environment.Exit(0);
         }

         Console.CancelKeyPress += SpecifyName;
         server.Start();
      }
   }

   class HttpServer
   {
      private HttpListener _listener;
      private string _port;

      public HttpServer(string port)
      {
         _port = port;
         _listener = new HttpListener();
         _listener.Prefixes.Add(string.Format(_port));
      }

      public void Start()
      {
         _listener.Start();
         Console.WriteLine("Сервер запущен, порт: {0}. Ожидание запросов...", _port);

         while (_listener.IsListening)
         {
            try
            {
               // Синхронное ожидание запроса
               HttpListenerContext context = _listener.GetContext();
               ProcessRequest(context);
            }
            catch (HttpListenerException)
            {
               // Сервер остановлен
               break;
            }
            catch (Exception ex)
            {
               Console.WriteLine("Ошибка обработки запроса: {0}", ex.Message);
            }
         }
      }

      public void Stop()
      {
         _listener.Stop();
         Console.WriteLine("Сервер остановлен");
      }

      private void ProcessRequest(HttpListenerContext context)
      {
         HttpListenerRequest request = context.Request;
         HttpListenerResponse response = context.Response;
         Console.WriteLine("{0} {1}", request.HttpMethod, request.Url.PathAndQuery);
         string responseString;
         int statusCode = 200;
         if (request.HttpMethod == "GET")
         {
            responseString = HandleGet(request);
         }
         else if (request.HttpMethod == "POST")
         {
            responseString = HandlePost(request);
         }
         else if (request.HttpMethod == "PUT")
         {
            responseString = HandlePut(request);
         }
         else if (request.HttpMethod == "DELETE")
         {
            responseString = HandleDelete(request);
         }
         else
         {
            statusCode = 405;
            // Метод не поддерживается
            responseString = "Метод не поддерживается";
         }

         // Формирование ответа
         byte[] buffer = Encoding.UTF8.GetBytes(responseString);
         response.ContentLength64 = buffer.Length;
         response.ContentType = "application/json; charset=utf-8";
         response.StatusCode = statusCode;
         using (Stream output = response.OutputStream)
         {
            output.Write(buffer, 0, buffer.Length);
         }
      }

      private string HandleGet(HttpListenerRequest request)
      {
         return "{\"method\": \"GET\", \"path\": \"" + request.Url.PathAndQuery + "\", \"status\": \"success\"}";
      }

      private string HandlePost(HttpListenerRequest request)
      {
         if (!request.HasEntityBody)
         {
            return "{\"error\": \"Нет данных в теле запроса\"}";
         }

         using (Stream body = request.InputStream)
         using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
         {
            string bodyText = reader.ReadToEnd();
            return "{\"method\": \"POST\", \"data\": \"" + bodyText + "\", \"status\": \"received\"}";
         }
      }

      private string HandlePut(HttpListenerRequest request)
      {
         if (!request.HasEntityBody)
         {
            return "{\"error\": \"Нет данных в теле запроса\"}";
         }

         using (Stream body = request.InputStream)
         using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
         {
            string bodyText = reader.ReadToEnd();
            return "{\"method\": \"PUT\", \"data\": \"" + bodyText + "\", \"status\": \"updated\"}";
         }
      }

      private string HandleDelete(HttpListenerRequest request)
      {
         return "{\"method\": \"DELETE\", \"path\": \"" + request.Url.PathAndQuery + "\", \"status\": \"deleted\"}";
      }
   }
}