namespace CoachingServices.src.console
{
    public class ColoredLog
    {
        public static void Log(string color, string message, bool isFullLine)
        {
            if (Enum.TryParse(color, true, out ConsoleColor consoleColor))
            {
                Console.ForegroundColor = consoleColor;
                if (isFullLine)
                    Console.WriteLine(message);
                else
                    Console.Write(message);

                Console.ResetColor();
            }
            else
            {
                Console.WriteLine($"Invalid color: {color}");
            }
        }
    }
}
