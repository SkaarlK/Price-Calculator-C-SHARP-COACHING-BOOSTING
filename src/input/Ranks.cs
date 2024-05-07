namespace CoachingServices.src.inputs
{
    public class Ranks
    {
        public LinkedList<string> list = new();
        public Ranks()
        {
            foreach (string rank in Program.allRanks)
            {
                bool isSingleDivisionedRank = Program.singleDivisionRanks.Contains(rank);
                if (isSingleDivisionedRank)
                {
                    list.AddLast($"{rank}_1");
                    continue;
                }
                for (int i = 4; i > 0; i--)
                {
                    list.AddLast($"{rank}_{i}");
                }
            }
        }
    }
    public class Rank(int value, string message, List<string> options) : Input(value, message, options) { }
}
