using CoachingServices.src.calculator;
using CoachingServices.src.inputs;
using CoachingServices.src.console;

class Program
    {
    public static readonly Dictionary<string, Dictionary<int, double>> rankPrices = new()
    {
        {
            "Iron", new Dictionary<int, double>()
            {
                { 4, 4.50 },
                { 3, 4.75 },
                { 2, 5.0 },
                { 1, 5.25 }
            }
        },
        {
            "Bronze", new Dictionary<int, double>()
            {
                { 4, 6.0 },
                { 3, 6.50 },
                { 2, 7.00 },
                { 1, 7.50 }
            }
        },
        {
            "Silver", new Dictionary<int, double>()
            {
                { 4, 8.50 },
                { 3, 9.50 },
                { 2, 10.50 },
                { 1, 11.50 }
            }
        },
        {
            "Gold", new Dictionary<int, double>()
            {
                { 4, 13.0 },
                { 3, 14.50 },
                { 2, 16.0 },
                { 1, 17.50 }
            }
        },
        {
            "Platinum", new Dictionary<int, double>()
            {
                { 4, 20.0 },
                { 3, 22.50 },
                { 2, 25.0 },
                { 1, 27.50 }
            }
        },
        {
            "Emerald", new Dictionary<int, double>()
            {
                { 4, 30.0 },
                { 3, 30.0 },
                { 2, 35.0 },
                { 1, 37.50 }
            }
            },
        {
            "Diamond", new Dictionary<int, double>()
            {
                { 4, 45.00 },
                { 3, 60.00 },
                { 2, 85.0 },
                { 1, 115.0 }
            }
            },
        {
            "Master", new Dictionary<int, double>()
            {
                { 1, 150.0 }
            }
        },
        {
            "Grandmaster", new Dictionary<int, double>()
            {
                { 1, 235.0 }
            }
        },
        {
            "Challenger", new Dictionary<int, double>()
            {
                { 1, 0.0 }
            }
        }
    };

    public static readonly string rankLabel = "Select your current rank:\n";
    public static readonly string divisionsLabel = "Select your current division:\n";
    public static readonly string targetRankLabel = "Select your average points earned per win:\n";
    public static readonly string targetDivisionLabel = "Select your target division:\n";

    public static readonly string lpGainRangesLabel = "Select your average points earned per win:\n";
    public static readonly List<string> lpGainRanges = ["14-", "15-18", "19-24", "25-29", "30+"];

    public static readonly string serversLabel = "Select your server:\n";
    public static readonly List<string> servers = ["EUW", "EUNE", "BR", "LAN", "NA"];

    public static readonly string queueTypesLabel = "Select your queue type:\n";
    public static readonly List<string> queueTypes = ["Solo", "Duo", "Flex/solo", "Flex/pre"];

    public static readonly string highestRank = rankPrices.Last().Key;

    public static readonly Dictionary<int, double> lpGainRangesPrices = MakeIndexedDictionary([1.0, 1.1, 1.2, 1.3, 1.4], false);
    public static readonly Dictionary<int, double> serverPrices = MakeIndexedDictionary([1.0, 1.1, 1.2, 1.3, 1.4], false);
    public static readonly Dictionary<int, double> queueTypesPrices = MakeIndexedDictionary([1.0, 1.1, 1.2, 1.3, 1.4], false);

    static void Main()
    {
        Calculator calculator = new(InitializeInputsFields());
        ColoredLog.Log("Yellow", $"The price is: {calculator.price}", true);

        //HACK: console won't close after calculation
        Console.ReadLine();
    }

    static Inputs InitializeInputsFields()
    {
        Rank rank = new(0, rankLabel, Ranks.GetAllRanks());
        Division division = new(0, divisionsLabel, Divisions.GetAllDivisions(rank));

        Rank targetRank = new(0, targetRankLabel, Ranks.GetOnlySelectableRanks(rank, division));
        Division targetDivision = new(0, targetDivisionLabel, Divisions.GetOnlySelectableDivisions(rank, division, targetRank));

        AverageLeaguePoints averageLPGain = new(0, lpGainRangesLabel, lpGainRanges);
        Server server = new(0, serversLabel, servers);
        Queue queue = new(0, queueTypesLabel, queueTypes);

        ColoredLog.Log("Yellow", "From ", false);
        ColoredLog.Log("Green", $"{rank} {division}", false);
        ColoredLog.Log("Yellow", $" to ", false);
        ColoredLog.Log("Green", $"{targetRank} {targetDivision}", true);

        return new Inputs() { rank = rank, division = division, targetRank = targetRank, targetDivision = targetDivision, averageLPGain = averageLPGain, server = server, queue = queue };
    }

    public static Dictionary<int, T> MakeIndexedDictionary<T>(List<T> entries, bool reverse)
    {
        Dictionary<int, T> dict = [];
        if (reverse)
            entries = entries.ToArray().Reverse().ToList();

        for (int i = 1; i <= entries.Count; i++)
        {
            dict.Add(i, entries[i - 1]);
        }
        return dict;
    }
}

