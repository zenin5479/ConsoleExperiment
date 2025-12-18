using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace ConsoleExperiment
{
   internal class Program
   {
      static void Main()
      {
         
         //BuildSynchronousServer();
         //CreategSynchronousServer();

         Console.ReadKey();
      }

      static void BuildSynchronousServer()
      {
         // Создаём экземпляр HttpListener
         HttpListener listener = new HttpListener();

         // Добавляем префиксы (адреса, на которых сервер будет слушать запросы)
         // Можно указать несколько префиксов
         listener.Prefixes.Add("http://localhost:8080/");
         listener.Prefixes.Add("http://127.0.0.1:8080/");

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
               Console.WriteLine($"Получен запрос: {request.HttpMethod} {request.Url}");

               // Формируем ответ
               string responseString = "<html><body><h1>Привет от синхронного сервера!</h1>" +
                                       $"<p>Время: {DateTime.Now}</p></body></html>";
               byte[] buffer = Encoding.UTF8.GetBytes(responseString);

               // Настраиваем ответ
               response.ContentLength64 = buffer.Length;
               response.ContentType = "text/html; charset=UTF-8";
               response.StatusCode = (int)HttpStatusCode.OK;

               // Отправляем ответ клиенту
               using (var output = response.OutputStream)
               {
                  output.Write(buffer, 0, buffer.Length);
               }

               // Закрываем запрос (освобождаем ресурсы)
               context.Response.Close();
            }
         }
         catch (HttpListenerException ex)
         {
            Console.WriteLine($"Ошибка HttpListener: {ex.Message}");
         }
         catch (Exception ex)
         {
            Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
         }
         finally
         {
            // Останавливаем прослушивание и освобождаем ресурсы
            listener.Stop();
            listener.Close();
            Console.WriteLine("Сервер остановлен.");
         }


      }

      static void CreategSynchronousServer()
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
         int i = 0;
         while (i < request.Headers.Keys.Count)
         {
            string item = request.Headers.Keys[i];
            Console.WriteLine($"{item}:{request.Headers[item]}");
            i++;
         }

         // Получаем объект для установки ответа
         HttpListenerResponse response = context.Response;
         byte[] buffer = Encoding.UTF8.GetBytes("Hello METANIT");
         // Получаем поток ответа и пишем в него ответ
         response.ContentLength64 = buffer.Length;
         using (Stream output = response.OutputStream)
         {
            // Отправляем данные
            output.Write(buffer);
            output.Flush();

            // Останавливаем сервер
            server.Stop();
         }
      }
   }

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
}