using System;
using System.Collections.Generic;

namespace TaskSharp.Utils
{
    public static class ConsoleHelper
    {
        public static string ReadNonEmpty(string prompt)
        {
            string? input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine()?.Trim();
            } while (string.IsNullOrEmpty(input));
            return input;
        }

        public static DateTime? ReadDate(string prompt)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(s)) return null;
            if (DateTime.TryParse(s, out var d)) return d;
            Console.WriteLine("Data inválida. Pressione Enter para continuar.");
            Console.ReadLine();
            return null;
        }

        public static IEnumerable<string> ReadTags(string prompt)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(s)) return Array.Empty<string>();
            return s.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
    }
}
