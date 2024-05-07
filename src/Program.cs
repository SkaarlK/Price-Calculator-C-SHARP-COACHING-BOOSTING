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
        Rank rank = new(0, $"Select your current rank:\n", allRanks);

        // Some ranks only have division I, passing 1 as 'value' parameter skips selection by user and set it as initial value
        Division division =  singleDivisionRanks.Contains(rank.ToString()) ? new(1, "Select your current division:\n", singleDivision) : new(0, "Select your current division:\n", allDivisions);

        Rank targetRank = new(0, $"Select your target rank:\n", allRanks);

        // Some ranks only have division I, passing 1 as 'value' parameter skips selection by user and set it as initial value
        Division targetDivision = singleDivisionRanks.Contains(targetRank.ToString()) ? new(1, "Select your target division:\n", singleDivision) : new(0, "Select your target division:\n", allDivisions);

        AverageLeaguePoints averageLPGain = new(0, "Select your average points earned per win:\n", ["14-", "15-18", "19-24", "25-29", "30+"]);
        Server server = new(0, "Select your server:\n", ["EUW", "EUNE", "BR", "LAN", "NA"]);
        Queue queue = new(0, "Select your queue type:\n", ["Solo", "Duo", "Flex/solo", "Flex/pre"]);

        Calculator calculator = new(rank, division, targetRank, targetDivision, averageLPGain, server, queue);

        Console.WriteLine($"The price for this service is: {calculator.price}");
    }
}

