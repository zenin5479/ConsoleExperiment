using System;

namespace ConsoleExperiment
{
   class Program
   {
      static void Main()
      {


         Console.ReadKey();
      }

      private static int CalculateConsole(double input)
      {
         int iterationCount = 0; // Счётчик итераций

         while (true) // Бесконечный цикл
         {
            iterationCount++; // Увеличиваем счётчик на каждой итерации

            Console.WriteLine("Итерация №{0}", iterationCount);
            Console.WriteLine();

            // Здесь можно разместить любую логику, выполняемую на каждой итерации
            return iterationCount;
         }
      }
   }
}