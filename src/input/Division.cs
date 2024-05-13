using ServiceStack.Script;

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

        public static List<string> FilterLowerDivisions(string division)
        {
            List<string> allDivisions = ["I", "II", "III", "IV"];
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

        public static bool IsSingleDivision(string rank)
        {
            return prices[rank].Count == 1;
        }

        public static List<string> GetAllDivisionsFromRank(string rank)
        {
            return prices[rank].Keys.Select(k => RomenizeInt(k, !IsSingleDivision(rank))).ToList();
        }
        public static List<string> GetOnlyHigherDivisionsThan(string division)
        {
            return FilterLowerDivisions(division);
        }

        public static List<string> GetOnlySelectableDivisions(Rank rank, Division division, Rank targetRank)
        {
            return Ranks.IsTargetSameAsCurrentRank(rank, targetRank) ? GetOnlyHigherDivisionsThan(division.ToString()) : GetAllDivisionsFromRank(targetRank.ToString());
        }
    }
    public class Division(int value, string message, List<string> options) : Input(value, message, options)
    {
    }
}
