using CoachingServices.src.inputs;

namespace CoachingServices.src.calculator
{

    public class Calculator
    {
        public double price = 0;
        readonly Ranks OnlyRanksBetweenRange = new(new InRangeRanksFilterStrategy(), Program.rankPrices);

        public Calculator(Inputs data)
        {
            double basePrice = SumPrices(data.rank.ToString(), data.division.value, data.targetRank.ToString(), data.targetDivision.value);

            basePrice *= Program.lpGainRangesPrices[data.averageLPGain.value];
            basePrice *= Program.serverPrices[data.server.value];

            if (data.queue.value == 2)
            {
                basePrice *= Program.serverPrices[data.queue.value];
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
                Console.WriteLine($"Price: {price}");

                string[] loopValues = loopElo.Key.Split("_");
                string loopRank = loopValues[0];

                if (loopRank == Program.highestRank) break;

                string loopDivision = Divisions.RomenizeInt(int.Parse(loopValues[1]), Divisions.IsSingleDivision(rank));

                Console.WriteLine($"{loopRank} {loopDivision}: +{loopElo.Value}");
                Console.WriteLine($"");
                price += loopElo.Value;
            }

            return price;
        }

        public static double MaxPrice() => SumDictionaryValues([.. Program.rankPrices.Values]);

        public static double SumDictionaryValues(Dictionary<int, double>[] dicts)
        {
            double sum = 0.0;
            foreach (var dict in dicts)
            {
                foreach (var pair in dict)
                    sum += pair.Value;
            }
            return sum;
        }

    }
}
