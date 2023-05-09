using System.Text.RegularExpressions;

namespace UI.ConsoleHandlers
{
    public static class ConsoleHandler
    {
        public static void Clear()
        {
            Console.Write("\x1B[2J");
            Console.SetCursorPosition(0, 0);
            Console.Write("\x1B[3J");
        }
        
        public static void Print(string? message)
        {
            Console.WriteLine(message);
        }
        
        public static void PrintInfo(string? message)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        
        public static void PrintCaption(string? message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();        }
        
        public static void RaiseSuccess(string? message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();        }

        public static void RaiseError(string? error)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();        }

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

                RaiseError("Invalid input!");
            }
        }


        public static void PrintCollection<T>(List<T> collection)
        {
            if (collection.Count == 0)
            {
                RaiseError($"No {typeof(T).Name} found.");
                return;
            }

            var colors = new[] { ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.DarkCyan};
    
            var currentColorIndex = 0;
    
            foreach (var element in collection)
            {
                Console.ForegroundColor = colors[currentColorIndex];
        
                Print(element?.ToString());
        
                currentColorIndex++;
        
                if (currentColorIndex >= colors.Length)
                {
                    currentColorIndex = 0;
                }
            }

            Console.ResetColor();
        }

        public static double AskForDouble(string caption)
        {
            PrintCaption(caption);
            while (true)
            {
                var input = Console.ReadLine();
                if (double.TryParse(input, out double result))
                {
                    if (result > 0)
                    {
                        return result;
                    }
                    
                    RaiseError("Input must be a positive number!");
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

                RaiseError("Invalid postal code!");
            }
        }
    }   
}