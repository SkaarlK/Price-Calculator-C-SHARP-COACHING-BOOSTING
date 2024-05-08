using CoachingServices.src.inputs;

namespace CoachingServices.src.calculator
{

    public class Calculator
    {
        public double price = 0;
        readonly Ranks OnlyRanksBetweenRange = new(new DefaultRankFilterStrategy(), Program.rankPrices);

        private Inputs inputs;
        public Calculator(Inputs data)
        {
            inputs = data;
            double basePrice = SumPrices(inputs.rank.ToString(), inputs.division.value, inputs.targetRank.ToString(), inputs.targetDivision.value);

            basePrice *= Program.lpGainWeight[inputs.averageLPGain.value];
            basePrice *= Program.serverWeight[inputs.server.value];

            if (inputs.queue.value == 2)
            {
                basePrice *= Program.serverWeight[inputs.queue.value];
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
                if (loopElo.Key == $"{targetRank}_{targetDivision}") break;
                price += loopElo.Value;
            }

            return price;
        }

    }
}
