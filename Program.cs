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
         int iterationCount = 0;
         while (true)
         {
            iterationCount++;
            double result = input * iterationCount;
            Console.Write(result);
            Console.WriteLine("\nИтерация: {0}", iterationCount);
            return result;
         }
      }
   }
}