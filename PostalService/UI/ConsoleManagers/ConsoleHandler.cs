using System.Text.RegularExpressions;
using Core.Models;

namespace UI.ConsoleManagers
{
    public static class ConsoleHandler
    {
        public static void Print(string? message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }
        
        public static void PrintInfo(string? message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        public static void PrintCaption(string? message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        
        public static void RaiseSuccess(string? message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void RaiseError(string? error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static string AskForString(string caption, string format)
        {
            PrintCaption(caption);

            while (true)
            {
                var input = Console.ReadLine();

                if (input != null && Regex.IsMatch(input, format))
                {
                    return input;
                }

                RaiseError($"Incorrect format! {format}");
            }
        }
        
        public static T AskForEnum<T>(string caption) where T : struct, Enum
        {
            PrintCaption(caption);
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                PrintInfo($"  => {(int)value}. {value}");
            }

            while (true)
            {
                var input = Console.ReadLine();
                if (Enum.TryParse(input, out T result))
                {
                    return result;
                }
                else
                {
                    RaiseError("Invalid input!");
                }
            }
        }


        public static void PrintCollection<T>(List<T> collection)
        {
            foreach (var element in collection)
            {
                Print(element?.ToString());
            }
        }

        public static double AskForDouble(string caption)
        {
            PrintCaption(caption);
            while (true)
            {
                var input = Console.ReadLine();
                if (double.TryParse(input, out double result))
                {
                    return result;
                }
                else
                {
                    RaiseError("Invalid input!");
                }
            }
        }

        public static int AskForPostalCode(string caption)
        {
            PrintCaption(caption);
            while (true)
            {
                var input = Console.ReadLine();
                if (Regex.IsMatch(input, @"^\d{5}$"))
                {
                    return int.Parse(input);
                }
                else
                {
                    RaiseError("Invalid postal code!");
                }
            }
        }
    }   
}