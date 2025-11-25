using System;

// Отправка синхронных запросов с помощью HttpClient

namespace ConsoleExperiment
{
   internal class Program
   {
      static void Main()
      {
         // Reuse these instances as needed
         var httpClient = new HttpClient();
         var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

         // Make the sync GET request
         using (var request = new HttpRequestMessage(HttpMethod.Get, "https://api.example.com/user"))
         {
            var response = httpClient.Send(request);

            response.EnsureSuccessStatusCode();

            using var stream = response.Content.ReadAsStream();
            var user = JsonSerializer.Deserialize<User>(stream, jsonOptions);

            Console.WriteLine($"User {user.Name} is {user.Age} years old.");
         }

         Console.ReadKey();
      }
   }
}