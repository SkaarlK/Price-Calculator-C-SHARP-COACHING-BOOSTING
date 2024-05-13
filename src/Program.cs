﻿using CoachingServices.src.calculator;
using CoachingServices.src.inputs;

class Program
    {
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
    public static readonly Ranks allRanks = new(new AllRanksFilterStrategy(), rankPrices);
    public static readonly Ranks onlyGreaterRanks = new(new GreaterRanksFilterStrategy(), rankPrices);

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

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"The price is: {calculator.price}");
        Console.ReadLine();
    }

    static Inputs InitializeInputsFields()
    {
        Rank rank = new(0, rankLabel, [.. rankPrices.Keys]);

        Division division = new(Division.IsSingleDivision(rank) ? 1 : 0, divisionsLabel, Division.GetAllDivisions(rank));

        List<string> onlySelectableRanks = Ranks.ShrinkDivisions(onlyGreaterRanks.FilterRanks(rank.ToString(), division.value, highestRank, 1));

        Rank targetRank = new(0, targetRankLabel, onlySelectableRanks);

        List<string>  onlySelectableDivisions = IsTargetSameAsCurrentRank(rank, targetRank) ? Division.GetOnlyHigherDivisions(division) : Division.GetAllDivisions(targetRank);

        Division targetDivision = new(Division.IsSingleDivision(targetRank) ? 1 : 0, targetDivisionLabel, onlySelectableDivisions);

        AverageLeaguePoints averageLPGain = new(1, lpGainRangesLabel, lpGainRanges);
        Server server = new(1, serversLabel, servers);
        Queue queue = new(1, queueTypesLabel, queueTypes);

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"From ");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"{rank} {division}");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($" to ");

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"{targetRank} {targetDivision}");

        return new Inputs() { rank = rank, division = division, targetRank = targetRank, targetDivision = targetDivision, averageLPGain = averageLPGain, server = server, queue = queue };
    }

    public static Dictionary<int, T> MakeIndexedDictionary<T>(List<T> entries, bool reverse)
    {
        Dictionary<int, T> dict = [];
        for (int i = 1; i <= entries.Count; i++)
        {
            if (reverse)
                {
                entries = entries.ToArray().Reverse().ToList();
                dict.Add(i, entries[i - 1]);
                }
            else
                {
                dict.Add(i, entries[i - 1]);
                }
            }
        return dict;
    }

    
    static bool IsTargetSameAsCurrentRank(Rank rank, Rank target)
    {
        return rank.ToString() == target.ToString();
    }

    

    }

