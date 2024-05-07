using CoachingServices.src.inputs;

namespace CoachingServices.src.calculator
{

    public class Calculator
    {
        public double price = 0;

        private readonly Dictionary<int, double> lpGainWeight = Input.MakeIndexedNumDictionary([1.0, 1.1, 1.2, 1.3, 1.4]);
        private readonly Dictionary<int, double> serverWeight = Input.MakeIndexedNumDictionary([1.0, 1.1, 1.2, 1.3, 1.4]);
        private readonly double duoQueueWeight = 1.35;

        readonly Ranks OnlyRanksBetweenRange = new(new DefaultRankFilterStrategy());

        public Calculator(Rank rank, Division division, Rank targetRank, Division targetDivision, AverageLeaguePoints averageLPGain, Server server, Queue queue)
        {
            double basePrice = SumPrices(rank.ToString(), division.value, targetRank.ToString(), targetDivision.value);

            basePrice *= lpGainWeight[averageLPGain.value];
            basePrice *= serverWeight[server.value];

            if (queue.value == 2)
            {
                basePrice *= duoQueueWeight;
            }

            price = basePrice;
        }
        private double SumPrices(string rank, int division, string targetRank, int targetDivision)
        {
            //Base case: initial & target rank/division are both the same;
            if (rank == targetRank && division == targetDivision) return 0;

            //initialize loop variables, only start price sums if the desired starting elo already were reached inside the loop;
            double price = 0;

            Dictionary<string, double> rankPriceDict = OnlyRanksBetweenRange.FilterRanks(rank, division, targetRank, targetDivision);

            foreach (KeyValuePair<string, double> loopElo in rankPriceDict)
            {
                price += loopElo.Value;
            }

            return price;
        }

    }
}
