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
      static List<User> users = new List<User>();

      static void Main()
      {
         using (HttpListener listener = new HttpListener())
         {
            listener.Prefixes.Add("http://localhost:8080/");
            listener.Start();
            Console.WriteLine("Сервер запущен на http://localhost:8080/");

            while (true)
            {
               HttpListenerContext context = listener.GetContext();
               ProcessRequest(context);
            }
         }

         //BuildSynchronousServer();
         //CreategSynchronousServer();

         Console.ReadKey();
      }

      static void ProcessRequest(HttpListenerContext context)
      {
         try
         {
            var request = context.Request;
            var response = context.Response;

            response.ContentType = "application/json";
            string result = "";

            switch (request.HttpMethod)
            {
               case "GET":
                  result = HandleGet(request);
                  break;
               case "POST":
                  result = HandlePost(request);
                  break;
               case "PUT":
                  result = HandlePut(request);
                  break;
               case "DELETE":
                  result = HandleDelete(request);
                  break;
               default:
                  result = JsonConvert.SerializeObject(new { error = "Метод не поддерживается" });
                  response.StatusCode = 405;
                  break;
            }

            byte[] buffer = Encoding.UTF8.GetBytes(result);
            response.ContentLength64 = buffer.Length;
            using (Stream output = response.OutputStream)
            {
               output.Write(buffer, 0, buffer.Length);
            }
         }
         catch (Exception ex)
         {
            Console.WriteLine($"Ошибка: {ex.Message}");
         }
      }

      static string HandleGet(HttpListenerRequest request)
      {
         string id = request.QueryString["id"];

         if (string.IsNullOrEmpty(id))
         {
            return JsonConvert.SerializeObject(users);
         }
         else
         {
            var user = users.Find(u => u.Id == int.Parse(id));
            return user != null
            ? JsonConvert.SerializeObject(user)
                : JsonConvert.SerializeObject(new { error = "Пользователь не найден" });
         }
      }

      static string HandlePost(HttpListenerRequest request)
      {
         string body = ReadRequestBody(request);
         var newUser = JsonConvert.DeserializeObject<User>(body);
         newUser.Id = users.Count + 1;
         users.Add(newUser);

         return JsonConvert.SerializeObject(new
         {
            message = "Пользователь создан",
            user = newUser
         });
      }

      static string HandlePut(HttpListenerRequest request)
      {
         string body = ReadRequestBody(request);
         var updatedUser = JsonConvert.DeserializeObject<User>(body);
         var existingUser = users.Find(u => u.Id == updatedUser.Id);

         if (existingUser == null)
         {
            return JsonConvert.SerializeObject(new { error = "Пользователь не найден" });
         }

         existingUser.Name = updatedUser.Name;
         existingUser.Email = updatedUser.Email;

         return JsonConvert.SerializeObject(new
         {
            message = "Пользователь обновлен",
            user = existingUser
         });
      }

      static string HandleDelete(HttpListenerRequest request)
      {
         string id = request.QueryString["id"];
         if (string.IsNullOrEmpty(id))
         {
            return JsonConvert.SerializeObject(new { error = "Не указан ID" });
         }

         var user = users.Find(u => u.Id == int.Parse(id));
         if (user == null)
         {
            return JsonConvert.SerializeObject(new { error = "Пользователь не найден" });
         }

         users.Remove(user);
         return JsonConvert.SerializeObject(new { message = "Пользователь удален" });
      }

      static string ReadRequestBody(HttpListenerRequest request)
      {
         using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
         {
            return reader.ReadToEnd();
         }
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

   public class User
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Email { get; set; }
   }
}