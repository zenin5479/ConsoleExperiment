using System;
using System.IO;
using System.Net; //добавить
using System.Text;

//public partial class Form1 : Form
//{
//   HttpListener server;
//   bool flag = true;
//   public Form1()
//   {
//      InitializeComponent();
//   }
//   private void button1_Click(object sender, EventArgs e)
//   {
//      //ресурс, который будет запрашивать пользователь
//      string uri = @"http://192.168.10.1:8080/say/";
//      StartServer(uri);
//   }
//   private void StartServer(string prefix)
//   {
//      server = new HttpListener();
//      // текущая ос не поддерживается
//      if (!HttpListener.IsSupported) return;
//      //добавление префикса (say/)
//      //обязательно в конце должна быть косая черта
//      if (string.IsNullOrEmpty(prefix))
//         throw new ArgumentException("prefix");
//      server.Prefixes.Add(prefix);
//      //запускаем север
//      server.Start();
//      this.Text = "Сервер запущен!";
//      //сервер запущен? Тогда слушаем входящие соединения
//      while (server.IsListening)
//      {
//         //ожидаем входящие запросы
//         HttpListenerContext context = server.GetContext();
//         //получаем входящий запрос
//         HttpListenerRequest request = context.Request;
//         //обрабатываем POST запрос
//         //запрос получен методом POST (пришли данные формы)
//         if (request.HttpMethod == "POST")
//         {
//            //показать, что пришло от клиента
//            ShowRequestData(request);
//            //завершаем работу сервера
//            if (!flag) return;
//         }
//         //формируем ответ сервера:
//         //динамически создаём страницу
//         string responseString =
//            @"<!DOCTYPE HTML><html><head></head><body><form method=""post"" action=""say""><p><b>Name: </b><br><input type=""text"" name=""myname"" size=""40""></p><p><input type=""submit"" value=""send""></p></form></body></html>";
//         //отправка данных клиенту
//         HttpListenerResponse response = context.Response;
//         response.ContentType = "text/html; charset=UTF-8";
//         byte[] buffer = Encoding.UTF8.GetBytes(responseString);
//         response.ContentLength64 = buffer.Length;
//         using (Stream output = response.OutputStream)
//         {
//            output.Write(buffer, 0, buffer.Length);
//         }
//      }
//   }
//   private void ShowRequestData(HttpListenerRequest request)
//   {
//      //есть данные от клиента?
//      if (!request.HasEntityBody) return;
//      //смотрим, что пришло
//      using (Stream body = request.InputStream)
//      {
//         using (StreamReader reader = new StreamReader(body))
//         {
//            string text = reader.ReadToEnd();
//            //оставляем только имя
//            text = text.Remove(0, 7);
//            //преобразуем %CC%E0%EA%F1 -> Макс
//            text = System.Web.HttpUtility.UrlDecode(text, Encoding.UTF8);
//            //выводим имя
//            MessageBox.Show("Ваше имя: " + text);
//            flag = true;
//            //останавливаем сервер
//            if (text == "stop")
//            {
//               server.Stop();
//               this.Text = "Сервер остановлен!";
//               flag = false;
//            }
//         }
//      }
//   }
//}
//}