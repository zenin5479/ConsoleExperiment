using System;

namespace ConsoleExperiment
{
   class Program
   {
      static void Main()
      {
         double inputValue = 1024;
         //double result = CalculateConsole(inputValue);
         //Console.WriteLine(result);

         Calculate(inputValue);

         //long iterations = 0;
         //while (true)
         //{
         //   iterations++;
         //   double result = inputValue * iterations;
         //   Console.WriteLine("Итерация: {0}", result);
         //}
      }

      private static void Calculate(double input)
      {
         // Используем long, чтобы избежать переполнения int
         long iterations = 0;
         // Бесконечный цикл
         while (true)
         {
            iterations++;
            double result = input * iterations;
            Console.WriteLine("Итерация: {0}", result);
         }
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
            Console.WriteLine("Итерация: {0}", iterationCount);
            return result;
         }
      }
   }
}