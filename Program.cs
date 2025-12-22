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
         HttpListener server;
         bool flag = true;

         //ресурс, который будет запрашивать пользователь
         string uri = @"http://192.168.10.1:8080/say/";

         void StartServer(string prefix)
         {
            server = new HttpListener();
            // текущая ос не поддерживается
            if (!HttpListener.IsSupported)
            {
               return;
            }

            //добавление префикса (say/)
            //обязательно в конце должна быть косая черта
            if (string.IsNullOrEmpty(prefix))
            {
               throw new ArgumentException("prefix");
            }

            server.Prefixes.Add(prefix);
            //запускаем север
            server.Start();
            Console.WriteLine("Сервер запущен!");
            //сервер запущен? Тогда слушаем входящие соединения
            while (server.IsListening)
            {
               //ожидаем входящие запросы
               HttpListenerContext context = server.GetContext();
               //получаем входящий запрос
               HttpListenerRequest request = context.Request;
               //обрабатываем POST запрос
               //запрос получен методом POST (пришли данные формы)
               if (request.HttpMethod == "POST")
               {
                  //показать, что пришло от клиента
                  ShowRequestData(request);
                  //завершаем работу сервера
                  if (!flag) return;
               }
               //формируем ответ сервера:
               //динамически создаём страницу
               string responseString =
                  @"<!DOCTYPE HTML><html><head></head><body><form method=""post"" action=""say""><p><b>Name: </b><br><input type=""text"" name=""myname"" size=""40""></p><p><input type=""submit"" value=""send""></p></form></body></html>";
               //отправка данных клиенту
               HttpListenerResponse response = context.Response;
               response.ContentType = "text/html; charset=UTF-8";
               byte[] buffer = Encoding.UTF8.GetBytes(responseString);
               response.ContentLength64 = buffer.Length;
               using (Stream output = response.OutputStream)
               {
                  output.Write(buffer, 0, buffer.Length);
               }
            }
         }




         StartServer(uri);

         //CreateSynchronousServerOne();
         //CreateSynchronousServerTwo();
         //CreateSynchronousServerThree();

         Console.ReadKey();
      }

      // Метод работет без прав администратора 
      static void CreateSynchronousServerOne()
      {
         // HttpListener. HTTP-сервер
         //https://metanit.com/sharp/net/7.1.php?ysclid=mjgxsc9r5p234582411

         HttpListener server = new HttpListener();
         // Установка адресов прослушки
         server.Prefixes.Add("http://127.0.0.1:8888/connection/");
         //server.Prefixes.Add("http://localhost:8080/");
         // Начинаем прослушивать входящие подключения
         server.Start();
         Console.WriteLine("Сервер запущен. Ожидание подключений...");

         // Получаем контекст
         HttpListenerContext context = server.GetContext();
         // Получаем данные запроса
         HttpListenerRequest request = context.Request;
         Console.WriteLine("Адрес приложения: {0}", request.LocalEndPoint);
         Console.WriteLine("Адрес клиента: {0}", request.RemoteEndPoint);
         Console.WriteLine(request.RawUrl);
         Console.WriteLine("Запрошен адрес: {0}", request.Url);
         Console.WriteLine("Заголовки запроса:");
         int i = 0;
         while (i < request.Headers.Keys.Count)
         {
            string item = request.Headers.Keys[i];
            Console.WriteLine("{0}:{1}", item, request.Headers[item]);
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
            server.Close();
         }
      }

      static void CreateSynchronousServerTwo()
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

      static void CreateSynchronousServerThree()
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
}