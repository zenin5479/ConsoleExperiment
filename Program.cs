using System;

namespace ConsoleExperiment
{
   class Program
   {
      static void Main()
      {
         // Выполняем расчёт с использованием консоли

         double inputValue = 256;
         int result = CalculateConsole(inputValue);


         //long iterations = 0; // используем long, чтобы избежать переполнения int
         //while (true)         // бесконечный цикл
         //{
         //   iterations++;

         //   // Периодически выводим значение счётчика
         //   if (iterations % 1_000_000 == 0)
         //   {
         //      Console.WriteLine("Выполнено итераций: {0:N0}", iterations);
         //   }

         //   // Здесь можно разместить любую другую логику
         //}

         Console.ReadKey();
      }

      private static int CalculateConsole(double input)
      {
         // Счётчик итераций
         int iterationCount = 0; 
// Бесконечный цикл
         while (true) 
         {
            iterationCount++; // Увеличиваем счётчик на каждой итерации

            Console.WriteLine("Итерация: {0}", iterationCount);
            Console.WriteLine();

            // Здесь можно разместить любую логику, выполняемую на каждой итерации
            return iterationCount;
         }
      }
   }
}