using CoachingServices.src.calculator;
using CoachingServices.src.inputs;

class Program
    {
        public static List<string> ranks = ["Iron", "Bronze", "Silver", "Gold", "Platinum", "Emerald", "Diamond", "Master", "Grandmaster", "Challenger"];
        public static List<string> singleDivisionRanks = ["Master", "Grandmaster", "Challenger"];
        public static List<string> divisions = ["I", "II", "III", "IV"];
        public static readonly Dictionary<string, Dictionary<int, double>> rankPrices = new()
        {
            {
                "Iron", new Dictionary<int, double>()
                {
                    { 4, 3.25 },
                    { 3, 3.5 },
                    { 2, 3.75 },
                    { 1, 4.0 }
                }
            },
            {
                "Bronze", new Dictionary<int, double>()
                {
                    { 4, 4.50 },
                    { 3, 5.0 },
                    { 2, 5.50 },
                    { 1, 6.0 }
                }
            },
            {
                "Silver", new Dictionary<int, double>()
                {
                    { 4, 7.50 },
                    { 3, 8.0 },
                    { 2, 9.5 },
                    { 1, 10.0 }
                }
            },
            {
                "Gold", new Dictionary<int, double>()
                {
                    { 4, 10.50 },
                    { 3, 11.25 },
                    { 2, 12.0 },
                    { 1, 13.75 }
                }
            },
            {
                "Platinum", new Dictionary<int, double>()
                {
                    { 4, 14.0 },
                    { 3, 15.0 },
                    { 2, 16.0 },
                    { 1, 18.0 }
                }
            },
            {
                "Emerald", new Dictionary<int, double>()
                {
                    { 4, 20.0 },
                    { 3, 22.0 },
                    { 2, 24.0 },
                    { 1, 26.0 }
                }
                },
            {
                "Diamond", new Dictionary<int, double>()
                {
                    { 4, 37.50 },
                    { 3, 42.50 },
                    { 2, 45.0 },
                    { 1, 75.0 }
                }
                },
            {
                "Master", new Dictionary<int, double>()
                {
                    { 1, 125.0 }
                }
            },
            {
                "Grandmaster", new Dictionary<int, double>()
                {
                    { 1, 250 }
                }
            },
            {
                "Challenger", new Dictionary<int, double>()
                {
                    { 1, 250.0 }
                }
            }
        };

        public static readonly Dictionary<int, double> lpGainWeight = MakeIndexedNumDictionary([1.0, 1.1, 1.2, 1.3, 1.4]);
        public static readonly Dictionary<int, double> serverWeight = MakeIndexedNumDictionary([1.0, 1.1, 1.2, 1.3, 1.4]);
        public static readonly Dictionary<int, double> duoQueueWeight = MakeIndexedNumDictionary([1.0, 1.1, 1.2, 1.3, 1.4]);

        public static readonly Ranks allRanks = new(new AllRanksFilterStrategy(), rankPrices);
        public static readonly Ranks onlyGreaterRanks = new(new GreaterRanksFilterStrategy(), rankPrices);

        private static Rank? rank;
        private static Division? division;

        private static Rank? targetRank;
        private static Division? targetDivision;

        private static AverageLeaguePoints? averageLPGain;
        private static Server? server;
        private static Queue? queue;

        static void Main()
        {
            Calculator calculator = new(InitializeInputsFields());

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"From {rank}{division} to {targetRank}{targetDivision}");
            Console.WriteLine($"The price for this service is: {calculator.price}");
            Console.ReadLine();
        }

        static Inputs InitializeInputsFields()
        {
            rank = new(0, $"Select your current rank:\n", ranks);

            // Some ranks only have division I, passing 1 as 'value' parameter skips selection by user and set it as initial value
            division = singleDivisionRanks.Contains(rank.ToString()) ? new(1, "Select your current division:\n", [divisions[0]]) : new(0, "Select your current division:\n", divisions);

            // After selecting current rank and division we only need to show greater ranks/division;
            targetRank = new(0, $"Select your target rank:\n", Ranks.ShrinkDivisions(onlyGreaterRanks.FilterRanks(rank.ToString(), division.value, "Challenger", 1)));

            // After selecting current division we only need to show greater divisions if the target rank is the same as current;
            List<string> onlyGreaterDivisions = rank.ToString() == targetRank.ToString() ? Ranks.FilterLowerDivisions(division.ToString()) : divisions;
            targetDivision = singleDivisionRanks.Contains(targetRank.ToString()) ? new(1, "Select your target division:\n", [divisions[0]]) : new(0, "Select your target division:\n", onlyGreaterDivisions);

            averageLPGain = new(1, "Select your average points earned per win:\n", ["14-", "15-18", "19-24", "25-29", "30+"]);
            server = new(1, "Select your server:\n", ["EUW", "EUNE", "BR", "LAN", "NA"]);
            queue = new(1, "Select your queue type:\n", ["Solo", "Duo", "Flex/solo", "Flex/pre"]);

            return new Inputs { rank = rank, division = division, targetRank = targetRank, targetDivision = targetDivision, averageLPGain = averageLPGain, server = server, queue = queue };
        }
        public static Dictionary<int, double> MakeIndexedNumDictionary(double[] entries)
        {
            Dictionary<int, double> dict = [];
            int i = 1;
            foreach (var entry in entries)
            {
                dict.Add(i++, entry);
            }
            return dict;
        }
    }

