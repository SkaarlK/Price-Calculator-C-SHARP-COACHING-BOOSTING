using ServiceStack.Script;

namespace CoachingServices.src.inputs
    {
    public class RankContext
    {
        public required Dictionary<string, double> List { get; set; }
        public string? Key { get; set; }
        public required string StartRank { get; set; }
        public required int StartDivision { get; set; }
        public required string EndRank { get; set; }
        public required int EndDivision { get; set; }
    }

    public interface IRankFilterStrategy
    {
        bool Filter(RankContext context);
    }

    public class CustomRankFilterStrategy(Func<RankContext, bool> customFilterLogic) : IRankFilterStrategy
    {
        private readonly Func<RankContext, bool> customFilterLogic = customFilterLogic;

        public bool Filter(RankContext context)
        {
            return customFilterLogic(context);
        }
    }

    public class InRangeRanksFilterStrategy : IRankFilterStrategy
    {
        public bool Filter(RankContext context)
        {
            int currentIndex = context.List.Keys.ToList().IndexOf(context.Key ?? "Iron_4");
            int startIndex = context.List.Keys.ToList().IndexOf($"{context.StartRank}_{context.StartDivision}");
            int endIndex = context.List.Keys.ToList().IndexOf($"{context.EndRank}_{context.EndDivision}");

            return currentIndex >= startIndex && currentIndex <= endIndex;
        }
    }

    public class GreaterRanksFilterStrategy : IRankFilterStrategy
    {
        public bool Filter(RankContext context)
        {
            int currentIndex = context.List.Keys.ToList().IndexOf(context.Key ?? "Iron_4");
            int startIndex = context.List.Keys.ToList().IndexOf($"{context.StartRank}_{context.StartDivision}");

            return currentIndex > startIndex;
        }
    }

    public class Ranks
    {
        public Dictionary<string, double> list = [];

        private readonly IRankFilterStrategy filterStrategy;

        public Ranks(IRankFilterStrategy filterStrategy, Dictionary<string, Dictionary<int, double>> rankPrices)
            {
            InitializeRanks(rankPrices);
            this.filterStrategy = filterStrategy;
            }

        public Dictionary<string, double> FilterRanks(string startRank, int startDivision, string endRank, int endDivision)
        {
            var context = new RankContext
            {
                List = list,
                StartRank = startRank,
                StartDivision = startDivision,
                EndRank = endRank,
                EndDivision = endDivision
            };

            var keys = list.Keys
                .Where(key =>
                {
                    context.Key = key;
                    return filterStrategy.Filter(context);
                })
                .ToList();

            Dictionary<string, double> filteredRanks = keys.ToDictionary(key => key, key => list[key]);

            return filteredRanks;
        }

        public static List<string> GetRanksFromElos(Dictionary<string, double> elos)
        {
            List<string> onlyRanks = [];
            foreach (var elo in elos)
            {
                string rank = elo.Key.Split('_')[0];
                if (!onlyRanks.Contains(rank))
                    onlyRanks.Add(rank);
            }
            return onlyRanks;
        }
        public static List<string> GetOnlySelectableRanks(string rank, int division)
        {
            Ranks onlyGreaterRanks = new(new GreaterRanksFilterStrategy(), Program.rankPrices);
            return GetRanksFromElos(onlyGreaterRanks.FilterRanks(rank, division, Program.highestRank, 1));
        }
        public static List<string> GetAllRanks()
        {
            return [.. Program.rankPrices.Keys];
        }
        public static bool IsTargetSameAsCurrentRank(Rank rank, Rank target)
        {
            return rank.ToString() == target.ToString();
        }

        public void InitializeRanks(Dictionary<string, Dictionary<int, double>> rankPrices)
        {
            foreach (string rank in Program.rankPrices.Keys)
            {
                for (int i = Program.rankPrices[rank].Count; i > 0; i--)
                {
                    if (Program.rankPrices[rank].Count == 1)
                        {
                            list.Add($"{rank}_1", rankPrices[rank][1]);
                            continue;
                        }
                    list.Add($"{rank}_{i}", rankPrices[rank][i]);
                }
            }
        }
    }

    public class Rank(int value, string message, List<string> options) : Input(value, message, options) { }
}

