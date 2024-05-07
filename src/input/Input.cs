﻿namespace CoachingServices.src.inputs
{
    public class Input
    {
        public override string ToString()
        {
            return Options[value];
        }
        public readonly int value;

        private readonly Dictionary<int, string> Options;

        public Input(int value, string message, List<string> options)
        {
            Options = MakeIndexedStringDictionary(options);

            if (value > 0)
            {
                this.value = value;
                return;
            }

            this.value = ReadOptionAndSanitize(message);

        }
        private void ClearLastLine()
        {
            int currentLineCursor = Console.CursorTop - 1;
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        public int ReadOptionAndSanitize(string message)
        {
            bool needToClearInvalidEntryLines = false;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();

            for (int i = 1; i <= Options.Count; i++)
            {
                Console.WriteLine($"{i}: {Options[i]}\n");
            }
            Console.ForegroundColor = ConsoleColor.Green;

            int option;
            while (true)
            {
                string answer = Console.ReadLine() ?? "";
                bool isNumeric = int.TryParse(answer, out option);

                if (isNumeric && option >= 1 && option <= Options.Count)
                {
                    break;
                }
                else
                {
                    needToClearInvalidEntryLines = true;
                    ClearLastLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please enter a number between 1 and " + Options.Count);
                }
            }

            ClearLastLine();
            if (needToClearInvalidEntryLines)
            {
                ClearLastLine();
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Selected: {Options[option]}\n");
            Console.ResetColor();

            return option;
        }

        public static Dictionary<int, string> MakeIndexedStringDictionary(List<string> entries)
        {
            Dictionary<int, string> dict = [];
            int i = 1;
            foreach (string entry in entries)
            {
                dict.Add(i++, entry);
            }
            return dict;
        }
        public static Dictionary<int, double> MakeIndexedNumDictionary(double[] entries)
        {
            Dictionary<int, double> dict = [];
            int i = 1;
            foreach (double entry in entries)
            {
                dict.Add(i++, entry);
            }
            return dict;
        }
    }
}