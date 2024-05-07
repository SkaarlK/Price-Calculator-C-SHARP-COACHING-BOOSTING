using CoachingServices.src.inputs;

namespace CoachingServices.src.calculator
{

    public class Calculator
    {
        public double price = 0;

        private readonly Dictionary<string, Dictionary<int, double>> rankPrices = new()
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

        private readonly Dictionary<int, double> lpGainWeight = Input.MakeIndexedNumDictionary([1.0, 1.1, 1.2, 1.3, 1.4]);
        private readonly Dictionary<int, double> serverWeight = Input.MakeIndexedNumDictionary([1.0, 1.1, 1.2, 1.3, 1.4]);
        private readonly double duoQueueWeight = 1.35;
        private readonly Ranks ranks = new();

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

            //concatenate rank_division and targetRank_targetDivision to match Ranks Ranks.list Linked List keys format;
            string serviceStartingElo = $"{rank}_{division}";
            string endingElo = $"{targetRank}_{targetDivision}";

            //initialize loop variables, only start price sums if the desired starting elo already were reached inside the loop;
            bool passedInitialEloFromLoop = false;
            double price = 0;

            foreach (string loopElo in ranks.list)
            {
                //initialize and parse the loop elo;
                string loopRank = loopElo.Split('_')[0];
                int loopDivision = int.Parse(loopElo.Split('_')[1]);

                //Side case: when start and end ranks and divisions are both the same, it means we don't need no calculate;
                if (loopElo == endingElo && loopDivision == division) break;

                //reached the desired point for starting sums in price;
                if (!passedInitialEloFromLoop && loopElo == serviceStartingElo) passedInitialEloFromLoop = true;

                if (passedInitialEloFromLoop) price += rankPrices[loopRank][loopDivision];

                //reached the target rank, stop sums;
                if (loopElo == endingElo) break;
            }

            return price;
        }

    }
}
