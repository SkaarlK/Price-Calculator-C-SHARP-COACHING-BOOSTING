using CoachingServices.src.calculator;
using CoachingServices.src.inputs;

class Program
    {
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

    //this can be a new class: weight
    public static readonly Dictionary<int, double> lpGainWeight = MakeIndexedDictionary([1.0, 1.1, 1.2, 1.3, 1.4]);
    public static readonly Dictionary<int, double> serverWeight = MakeIndexedDictionary([1.0, 1.1, 1.2, 1.3, 1.4]);
    public static readonly Dictionary<int, double> duoQueueWeight = MakeIndexedDictionary([1.0, 1.1, 1.2, 1.3, 1.4]);

    public static readonly Ranks allRanks = new(new AllRanksFilterStrategy(), rankPrices);
    public static readonly Ranks onlyGreaterRanks = new(new GreaterRanksFilterStrategy(), rankPrices);
    public static Inputs inputs;

    static void Main()
    {

        inputs = InitializeInputsFields();
        Calculator calculator = new(inputs);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"From ");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{inputs.rank} {inputs.division}");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($" to ");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{inputs.targetRank} {inputs.targetDivision}");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"The price is: {calculator.price}");
        Console.ReadLine();
    }

    static Inputs InitializeInputsFields()
    {
        Rank rank = new(0, $"Select your current rank:\n", Program.rankPrices.Keys.ToList());

        // Some ranks only have division I, passing 1 as 'value' parameter skips selection by user and set it as initial value
        Division division = singleDivisionRanks.Contains(rank.ToString()) ? new(1, "Select your current division:\n", [divisions[0]]) : new(0, "Select your current division:\n", divisions);

        // After selecting current rank and division we only need to show greater ranks/division;
        Rank targetRank = new(0, $"Select your target rank:\n", Ranks.ShrinkDivisions(onlyGreaterRanks.FilterRanks(rank.ToString(), division.value, "Challenger", 1)));

        // After selecting current division we only need to show greater divisions if the target rank is the same as current;
        List<string> onlyGreaterDivisions = rank.ToString() == targetRank.ToString() ? Ranks.FilterLowerDivisions(division.ToString()) : divisions;
        Division targetDivision = singleDivisionRanks.Contains(targetRank.ToString()) ? new(1, "Select your target division:\n", [divisions[0]]) : new(0, "Select your target division:\n", onlyGreaterDivisions);

        AverageLeaguePoints averageLPGain = new(1, "Select your average points earned per win:\n", ["14-", "15-18", "19-24", "25-29", "30+"]);
        Server server = new(1, "Select your server:\n", ["EUW", "EUNE", "BR", "LAN", "NA"]);
        Queue queue = new(1, "Select your queue type:\n", ["Solo", "Duo", "Flex/solo", "Flex/pre"]);

        return new() { rank = rank, division = division, targetRank = targetRank, targetDivision = targetDivision, averageLPGain = averageLPGain, server = server, queue = queue };
    }
    public static Dictionary<int, T> MakeIndexedDictionary<T>(List<T> entries)
    {
        Dictionary<int, T> dict = [];
        int i = 1;
        foreach (var entry in entries)
        {
            dict.Add(i++, entry);
        }
        return dict;
    }
}

