namespace CoachingServices.src.inputs
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

            //initial value higher than 0 skips manual input by user;
            if (value > 0)
            {
                this.value = value;
                return;
            }

            PromptQuestion(message);
            
            this.value = ReadAnswer();

        }
        private static void ClearLastLineFromTerminal()
        {
            int currentLineCursor = Console.CursorTop - 1;
            Console.SetCursorPosition(0, currentLineCursor);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        public void PromptQuestion(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);

            Console.ResetColor();

            for (int i = 1; i <= Options.Count; i++)
            {
                Console.WriteLine($"{i}: {Options[i]}\n");
            }
        }

        public int ReadAnswer()
            {
                int option;
                bool needToClearPreviousInvalidEntryLines = false;

                Console.ForegroundColor = ConsoleColor.Green;

                while (true)
                {
                    string answer = Console.ReadLine() ?? "";
                    bool isNumeric = int.TryParse(answer, out option);

                    if (isNumeric && option >= 1 && option <= Options.Count)
                    {
                        break;
                    }

                    needToClearPreviousInvalidEntryLines = true;

                    //Clears the output from Console.ReadLine;
                    ClearLastLineFromTerminal();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid option. Please enter a number between 1 and " + Options.Count);
                }

                //Clears the output from Console.ReadLine;
                ClearLastLineFromTerminal();

                if (needToClearPreviousInvalidEntryLines)
                {
                    //Clears the 'Invalid option' error message;
                    ClearLastLineFromTerminal();
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
    }
    public struct Inputs
        {
        public Rank rank;
        public Division division;
        public Rank targetRank;
        public Division targetDivision;
        public AverageLeaguePoints averageLPGain;
        public Server server;
        public Queue queue;
        }
    }