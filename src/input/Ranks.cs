using System;
using System.Collections.Generic;
using System.Linq;

namespace CoachingServices.src.inputs
{

    public interface IRankFilterStrategy
        {
        bool Filter(string key, string startRank, int startDivision, string endRank, int endDivision);
        }

    public class DefaultRankFilterStrategy : IRankFilterStrategy
        {
        public bool Filter(string key, string startRank, int startDivision, string endRank, int endDivision)
            {
            int currentIndex = Ranks.list.Keys.ToList().IndexOf(key);
            int startIndex = Ranks.list.Keys.ToList().IndexOf($"{startRank}_{startDivision}");
            int endIndex = Ranks.list.Keys.ToList().IndexOf($"{endRank}_{endDivision}");

            return currentIndex >= startIndex && currentIndex <= endIndex;
            }
        }

    public class GreatersRankFilterStrategy : IRankFilterStrategy
        {
        public bool Filter(string key, string startRank, int startDivision, string endRank, int endDivision)
            {
            int currentIndex = Ranks.list.Keys.ToList().IndexOf(key);
            int startIndex = Ranks.list.Keys.ToList().IndexOf($"{startRank}_{startDivision}");

            return currentIndex > startIndex;
            }
        }

    public class AllRankFilterStrategy : IRankFilterStrategy
        {
        public bool Filter(string key, string startRank, int startDivision, string endRank, int endDivision)
            {
            int currentIndex = Ranks.list.Keys.ToList().IndexOf(key);
            int startIndex = Ranks.list.Keys.ToList().IndexOf($"{startRank}_{startDivision}");

            return currentIndex > startIndex;
            }
        }

    public class Ranks
    {

        public static Dictionary<string, double> list = new();

        private IRankFilterStrategy filterStrategy;

        public Ranks(IRankFilterStrategy filterStrategy)
        {
            this.filterStrategy = filterStrategy;
        }

        public Dictionary<string, double> FilterRanks(string startRank, int startDivision, string endRank, int endDivision)
        {
            var keys = list.Keys
                .Where(key => filterStrategy.Filter(key, startRank, startDivision, endRank, endDivision))
                .ToList();

            Dictionary<string, double> filteredRanks = keys.ToDictionary(key => key, key => list[key]);

            return filteredRanks;
        }

        public static List<string> ShrinkDivisions(Dictionary<string, double> ranks)
        {
            List<string> shrinkedList = new();
            foreach (var rank in ranks)
            {
                string rankWithoutDivision = rank.Key.Split('_')[0];
                if (!shrinkedList.Contains(rankWithoutDivision))
                    shrinkedList.Add(rankWithoutDivision);
            }
            return shrinkedList;
        }

        public static List<string> FilterLowerDivisions(string division)
        {
            List<string> allDivisions = new List<string> { "I", "II", "III", "IV" };
            int index = allDivisions.IndexOf(division);
            if (index != -1)
            {
                return allDivisions.GetRange(0, index);
            }
            else
            {
                throw new ArgumentException("Invalid division");
            }
        }


        public static void InitializeRanks(Dictionary<string, Dictionary<int, double>> rankPrices)
        {
            foreach (string rank in Program.allRanks)
            {
                if (Program.singleDivisionRanks.Contains(rank))
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
    }
    public class Rank(int value, string message, List<string> options) : Input(value, message, options) { }
}

