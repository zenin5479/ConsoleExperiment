using System;
using System.Collections.Generic;

namespace ConsoleExperiment
{
   internal class Program
   {
      static void Main()
      {
         Dictionary<string, double> prices = new Dictionary<string, double>
         {
            {"Apple", 22.2},
            {"Banana", 12.8},
            {"Orange", 15.5}
         };

         // Способ 1: Использование enumerator
         Console.WriteLine("Использование enumerator:");
         Dictionary<string, double>.Enumerator enumerator = prices.GetEnumerator();
         try
         {
            while (enumerator.MoveNext())
            {
               Console.WriteLine("{0}: ${1}", enumerator.Current.Key, enumerator.Current.Value);
            }
         }
         finally
         {
            enumerator.Dispose();
         }

         // Способ 2: Использование foreach (проще)
         Console.WriteLine("\nИспользование foreach:");
         foreach (KeyValuePair<string, double> item in prices)
         {
            Console.WriteLine("{0}: ${1}", item.Key, item.Value);
         }

         Console.ReadKey();
      }
   }
}