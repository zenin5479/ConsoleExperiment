using System;

namespace ConsoleExperiment
{
   class Program
   {
      static void Main()
      {
         double inputValue = 1024;
         double result = CalculateConsole(inputValue);
         Console.WriteLine(result);
      }
      
      private static double CalculateConsole(double input)
      {
         // Счётчик итераций
         int iterationCount = 0;
         // Бесконечный цикл
         while (true)
         {
            // Увеличиваем счётчик на каждой итерации
            iterationCount++;
            double result = input * iterationCount;
            Console.Write(result);
            Console.WriteLine("\nИтерация: {0}", iterationCount);
            return result;
         }
      }
   }
}