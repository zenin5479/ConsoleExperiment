using System;
using System.Collections.Generic;

namespace ConsoleExperiment
{
   class Program
   {
      static void Main()
      {
         double inputValue = 1024;
         IEnumerable<double> result = CalculateConsole(inputValue);
         foreach (double num in result)
         {
            Console.WriteLine(num);
         }

      }

      private static IEnumerable<double> CalculateConsole(double input)
      {
         int iterationCount = 0;
         while (true)
         {
            iterationCount++;
            double result = input * iterationCount;
            Console.Write(result);
            Console.WriteLine("\nИтерация: {0}", iterationCount);
            yield return result;
         }
      }
   }
}