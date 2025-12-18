using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ConsoleExperiment
{
   class WebServer
   {
      private readonly HttpListener _listener = new HttpListener();
      private readonly Dictionary<string, dynamic> _dataStore = new Dictionary<string, dynamic>();
      private const string Url = "http://localhost:8080/";

      public WebServer()
      {
         if (!HttpListener.IsSupported)
            throw new NotSupportedException("HttpListener не поддерживается в этой ОС.");

         _listener.Prefixes.Add(Url);
      }

      public void Start()
      {
         _listener.Start();
         Console.WriteLine($"Сервер запущен на {Url}. Ожидание запросов...");

         while (true)
         {
            try
            {
               // Блокирующее ожидание входящего подключения
               HttpListenerContext context = _listener.GetContext();

               // Обработка запроса синхронно
               HandleRequest(context);
            }
            catch (HttpListenerException)
            {
               Console.WriteLine("Сервер остановлен.");
               break;
            }
            catch (Exception ex)
            {
               Console.WriteLine($"Ошибка при обработке запроса: {ex.Message}");
            }
         }
      }

      public void Stop()
      {
         _listener.Stop();
         _listener.Close();
         Console.WriteLine("Сервер остановлен.");
      }

      private void HandleRequest(HttpListenerContext context)
      {
         HttpListenerRequest request = context.Request;
         HttpListenerResponse response = context.Response;

         string method = request.HttpMethod;
         string path = request.Url.AbsolutePath.Trim('/');
         string responseString;
         int statusCode = 200;

         try
         {
            switch (method)
            {
               case "GET":
                  if (string.IsNullOrEmpty(path))
                  {
                     // GET / — возвращаем все данные
                     responseString = JsonConvert.SerializeObject(_dataStore, Formatting.Indented);
                  }
                  else if (_dataStore.ContainsKey(path))
                  {
                     // GET /id — возвращаем объект по ключу
                     responseString = JsonConvert.SerializeObject(_dataStore[path], Formatting.Indented);
                  }
                  else
                  {
                     responseString = "{\"error\": \"Объект не найден\"}";
                     statusCode = 404;
                  }
                  break;

               case "POST":
                  // Читаем тело запроса
                  string requestBody = ReadRequestBody(request);
                  dynamic data = JsonConvert.DeserializeObject(requestBody);
                  string newId = Guid.NewGuid().ToString("N")[..8]; // короткий ID
                  _dataStore[newId] = data;
                  responseString = $"{{\"id\": \"{newId}\", \"message\": \"Объект создан\"}}";
                  statusCode = 201;
                  break;

               case "PUT":
                  if (!string.IsNullOrEmpty(path) && _dataStore.ContainsKey(path))
                  {
                     string putBody = ReadRequestBody(request);
                     dynamic updatedData = JsonConvert.DeserializeObject(putBody);
                     _dataStore[path] = updatedData;
                     responseString = $"{{\"id\": \"{path}\", \"message\": \"Объект обновлён\"}}";
                  }
                  else
                  {
                     responseString = "{\"error\": \"Объект не найден для обновления\"}";
                     statusCode = 404;
                  }
                  break;

               case "DELETE":
                  if (!string.IsNullOrEmpty(path) && _dataStore.Remove(path))
                  {
                     responseString = $"{{\"id\": \"{path}\", \"message\": \"Объект удалён\"}}";
                  }
                  else
                  {
                     responseString = "{\"error\": \"Объект не найден для удаления\"}";
                     statusCode = 404;
                  }
                  break;

               default:
                  responseString = "{\"error\": \"Метод не поддерживается\"}";
                  statusCode = 405;
                  break;
            }
         }
         catch (JsonException)
         {
            responseString = "{\"error\": \"Некорректный JSON\"}";
            statusCode = 400;
         }
         catch (Exception ex)
         {
            responseString = $"{{\"error\": \"Внутренняя ошибка: {ex.Message}\"}}";
            statusCode = 500;
         }

         // Формируем ответ
         byte[] buffer = Encoding.UTF8.GetBytes(responseString);
         response.StatusCode = statusCode;
         response.ContentType = "application/json";
         response.ContentLength64 = buffer.Length;
         response.OutputStream.Write(buffer, 0, buffer.Length);
         response.OutputStream.Close();
      }

      private string ReadRequestBody(HttpListenerRequest request)
      {
         if (!request.HasEntityBody) return string.Empty;

         using (Stream body = request.InputStream)
         using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
         {
            return reader.ReadToEnd();
         }
      }
   }

// Пример запуска
   class Program
   {
      static void Main(string[] args)
      {
         WebServer server = new WebServer();

         // Запускаем сервер в отдельном потоке, чтобы можно было остановить по нажатию
         var serverThread = new System.Threading.Thread(server.Start);
         serverThread.Start();

         Console.WriteLine("Нажмите Enter для остановки сервера...");
         Console.ReadLine();

         server.Stop();
         serverThread.Join();
      }
   }
}