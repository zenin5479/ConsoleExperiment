using System;

namespace ConsoleExperiment
{
   class Program
   {
      static void Main()
      {
         // Выполняем расчёт с использованием консоли

         double inputValue = 256;
         //double result = CalculateConsole(inputValue);
         //Console.WriteLine(result);

         //double resul = Calculate(inputValue);
         //Console.WriteLine(resul);

         while (true)
         {
            Calculate(inputValue);
            //Console.WriteLine(resul);
         }

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
         // используем long, чтобы избежать переполнения int
         long iterations = 0; 
         // бесконечный цикл
         while (true)         
         {
            iterations++;

            double result = input * iterations;
            Console.WriteLine("\nИтерация: {0}", result);
            Console.WriteLine();
            return;
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
            Console.WriteLine("\nИтерация: {0}", iterationCount);
            Console.WriteLine();

            return result;
         }
      }
   }
}