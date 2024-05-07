using System;

namespace CoachingServices.src.inputs
{
    public class Ranks
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

        public Dictionary<string, double> list = new();
        public Dictionary<string, double> filteredlist = new();
        public Ranks()
        {
            foreach (string rank in Program.allRanks)
            {
                bool isSingleDivisionedRank = Program.singleDivisionRanks.Contains(rank);
                if (isSingleDivisionedRank)
                {
                    list.Add($"{rank}_1", rankPrices[rank][1]);
                    continue;
                }
                for (int i = 4; i > 0; i--)
                {
                    list.Add($"{rank}_{i}", rankPrices[rank][i]);
                }
            }
        }

        public Dictionary<string, double> FilterRanks(string startRank, int startDivision, string endRank, int endDivision)
         {
            // Get the keys of the ranks from start to end
            var keys = list.Keys
                .Where(key =>
                {
                    string[] parts = key.Split('_');
                    string rank = parts[0];
                    int division = int.Parse(parts[1]);

                    // Get the indices of the current, start, and end ranks and divisions
                    int currentIndex = list.Keys.ToList().IndexOf(key);
                    int startIndex = list.Keys.ToList().IndexOf($"{startRank}_{startDivision}");
                    int endIndex = list.Keys.ToList().IndexOf($"{endRank}_{endDivision}");

                    // Check if the current index is within the start and end indices
                    return currentIndex >= startIndex && currentIndex <= endIndex;
                })
                .ToList();

            // Create a new dictionary with only the filtered ranks
            Dictionary<string, double> filteredRanks = keys.ToDictionary(key => key, key => list[key]);

            filteredlist = filteredRanks;
            return filteredRanks;
         }
    }
    public class Rank(int value, string message, List<string> options) : Input(value, message, options) { }
}
