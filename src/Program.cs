using CoachingServices.src.calculator;
using CoachingServices.src.inputs;

class Program
{
    public static List<string> allRanks = ["Iron", "Bronze", "Silver", "Gold", "Platinum", "Emerald", "Diamond", "Master", "Grandmaster", "Challenger"];
    public static List<string> singleDivisionRanks = ["Master", "Grandmaster", "Challenger"];
    public static List<string> allDivisions = ["I", "II", "III", "IV" ];
    public static List<string> singleDivision = [ "I" ];
    static void Main()
   {
        Division division;
        Division targetDivision;

        Rank rank = new(0, $"Select your current rank:\n", allRanks);

        bool isRankSingleDivisioned = singleDivisionRanks.Contains(rank.ToString());

        if (isRankSingleDivisioned)
        {
            division = new(1, "Select your current division:\n", singleDivision);
        } 
        else
        {
            division = new(0, "Select your current division:\n", allDivisions);
        }


        Rank targetRank = new(0, $"Select your target rank:\n", allRanks);

        bool isTargetRankSingleDivisioned = singleDivisionRanks.Contains(targetRank.ToString());

        if (isTargetRankSingleDivisioned)
        {
            targetDivision = new(1, "Select your target division:\n", singleDivision);
        }
        else
        {
            targetDivision = new(0, "Select your target division:\n", allDivisions);
        }

        AverageLeaguePoints averageLPGain = new(0, "Select your average points earned per win:\n", ["14-", "15-18", "19-24", "25-29", "30+"]);
        Server server = new(0, "Select your server:\n", ["EUW", "EUNE", "BR", "LAN", "NA"]);
        Queue queue = new(0, "Select your queue type:\n", ["Solo", "Duo", "Flex/solo", "Flex/pre"]);

        Calculator calculator = new(rank, division, targetRank, targetDivision, averageLPGain, server, queue);

        Console.WriteLine($"The price for this service is: {calculator.price}");
    }
}

