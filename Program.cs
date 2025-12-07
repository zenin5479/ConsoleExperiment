using System;
using System.Collections.Generic;

// Отправка синхронных запросов с помощью HttpClient

namespace ConsoleExperiment
{
   internal class Program
   {
      static void Main()
      {
         Dictionary<string, double> prices = new Dictionary<string, double>
         {
            {"Apple", 1.2},
            {"Banana", 0.8},
            {"Orange", 1.5}
         };

         // Способ 1: Использование enumerator
         Console.WriteLine("Using enumerator:");
         var enumerator = prices.GetEnumerator();
         try
         {
            while (enumerator.MoveNext())
            {
               Console.WriteLine($"{enumerator.Current.Key}: ${enumerator.Current.Value}");
            }
         }
         finally
         {
            enumerator.Dispose();
         }

         // Способ 2: Использование foreach (проще)
         Console.WriteLine("\nUsing foreach:");
         foreach (var item in prices)
         {
            Console.WriteLine($"{item.Key}: ${item.Value}");
         }

         Console.ReadKey();
      }
   }
}