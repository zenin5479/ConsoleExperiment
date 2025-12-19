using System;
using System.IO;
using System.Net;
using System.Text;

namespace ConsoleExperiment
{
   class HttpServer
   {
      private HttpListener _listener;
      private int _port;

      public HttpServer(int port)
      {
         _port = port;
         _listener = new HttpListener();
         _listener.Prefixes.Add($"http://+:{_port}/");
      }

      public void Start()
      {
         _listener.Start();
         Console.WriteLine($"Сервер запущен, порт: {_port}. Ожидание запросов...");

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
               Console.WriteLine($"Ошибка обработки запроса: {ex.Message}");
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
         Console.WriteLine($"{request.HttpMethod} {request.Url.PathAndQuery}");
         string responseString;
         int statusCode = 200;
         switch (request.HttpMethod)
         {
            case "GET":
               responseString = HandleGet(request);
               break;
            case "POST":
               responseString = HandlePost(request);
               break;
            case "PUT":
               responseString = HandlePut(request);
               break;
            case "DELETE":
               responseString = HandleDelete(request);
               break;
            default:
               statusCode = 405;
               // Метод не поддерживается
               responseString = "Метод не поддерживается";
               break;
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

   // Точка входа
   class Program
   {
      static void Main()
      {
         int port = 8080;
         HttpServer server = new HttpServer(port);
         Console.WriteLine("Нажмите Ctrl+C для остановки сервера...");
         Console.CancelKeyPress += (sender, e) =>
         {
            e.Cancel = true;
            server.Stop();
            Environment.Exit(0);
         };

         server.Start();
      }
   }
}