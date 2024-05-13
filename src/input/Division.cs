using ServiceStack.Script;

namespace CoachingServices.src.inputs
{
    public class Division(int value, string message, List<string> options) : Input(value, message, options) {
        public static string RomenizeInt(int i, bool reverse)
        {
            Dictionary<int, string> romeNumbers = Program.MakeIndexedDictionary(["IV", "III", "II", "I"], reverse);
            return romeNumbers[i];
        }

        public static bool IsSingleDivision(Rank rank)
        {
            return Program.rankPrices[rank.ToString()].Count == 1;
        }

        public static List<string> GetAllDivisions(Rank rank)
        {
            return Program.rankPrices[rank.ToString()].Keys.Select(k => RomenizeInt(k, IsSingleDivision(rank))).ToList();
        }
        public static List<string> GetOnlyHigherDivisions(Division division)
        {
            return Ranks.FilterLowerDivisions(division.ToString());
        }
    }
}
