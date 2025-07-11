namespace GameManagement.Core.Games.Tennis
{
    public enum TennisScore
    {
        Love = 0,
        Fifteen = 15,
        Thirty = 30,
        Forty = 40,
        Advantage,
        Game
    }

    public static class TennisScoreExtensions
    {
        public static string ToDisplayString(this TennisScore score)
        {
            return score switch
            {
                TennisScore.Love => "0",
                TennisScore.Fifteen => "15",
                TennisScore.Thirty => "30",
                TennisScore.Forty => "40",
                TennisScore.Advantage => "AD",
                TennisScore.Game => "Game",
                _ => score.ToString()
            };
        }
    }
}