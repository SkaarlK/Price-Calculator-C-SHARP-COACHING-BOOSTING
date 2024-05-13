namespace CoachingServices.src.inputs
{
    public class Divisions()
    {
        private static readonly Dictionary<string, Dictionary<int, double>> prices = Program.rankPrices;

        public static string RomenizeInt(int i, bool reverse)
        {
            Dictionary<int, string> romeNumbers = Program.MakeIndexedDictionary(["I", "II", "III", "IV"], reverse);
            return romeNumbers[i];
        }
        public static bool IsSingleDivision(string rank)
        {
            return prices[rank].Count == 1;
        }

        public static List<string> GetOnlyHigherDivisionsThan(string division)
        {
            List<string> allDivisions = GetAllDivisions();
            int index = allDivisions.IndexOf(division);
            if (index != -1)
            {
                return allDivisions.GetRange(0, index);
            }
            else
            {
                //needs try catch block
                throw new ArgumentException("Invalid division");
            }
        }
        public static List<string> GetAllDivisions()
        {
            HashSet<string> allDivisions = [];

            foreach (var rank in prices)
            {
                foreach (var division in rank.Value.Keys)
                {
                    allDivisions.Add(RomenizeInt(division, !IsSingleDivision(rank.Key)));
                }
            }

            return [.. allDivisions];
         }
        public static List<string> GetAllDivisionsFromRank(string rank)
        {
            return prices[rank].Keys.Select(k => RomenizeInt(k, !IsSingleDivision(rank))).ToList();
        }
        public static List<string> GetOnlySelectableDivisions(string rank, string division, string targetRank)
        {
            return Ranks.IsTargetSameAsCurrentRank(rank, targetRank) ? GetOnlyHigherDivisionsThan(division) : GetAllDivisionsFromRank(targetRank);
        }
    }
    public class Division(int value, string message, List<string> options) : Input(value, message, options)
    {
    }
}
