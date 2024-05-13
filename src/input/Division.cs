using ServiceStack.Script;

namespace CoachingServices.src.inputs
{
    public class Divisions()
    {
        public static string RomenizeInt(int i, bool reverse)
            {
            Dictionary<int, string> romeNumbers = Program.MakeIndexedDictionary(["I", "II", "III", "IV"], reverse);
            return romeNumbers[i];
            }

        public static bool IsSingleDivision(string rank)
            {
            return Program.rankPrices[rank].Count == 1;
            }

        public static List<string> GetAllDivisions(Rank rank)
            {
            return Program.rankPrices[rank.ToString()].Keys.Select(k => RomenizeInt(k, !IsSingleDivision(rank.ToString()))).ToList();
            }
        public static List<string> GetOnlyHigherDivisions(Division division)
            {
            return Ranks.FilterLowerDivisions(division.ToString());
            }

        public static List<string> GetOnlySelectableDivisions(Rank rank, Division division, Rank targetRank)
            {
            return Ranks.IsTargetSameAsCurrentRank(rank, targetRank) ? GetOnlyHigherDivisions(division) : GetAllDivisions(targetRank);
            }
        }
    public class Division(int value, string message, List<string> options) : Input(value, message, options)
    {
    }
}
