using GameManagement.Core.Common;

namespace GameManagement.Core.Games.Tennis
{
    public class TennisPlayer : Player
    {
        public int Aces { get; private set; }
        public int DoubleFaults { get; private set; }
        public int GamesWon { get; private set; }
        public int SetsWon { get; private set; }

        public TennisPlayer(string name) : base(name)
        {
            Aces = 0;
            DoubleFaults = 0;
            GamesWon = 0;
            SetsWon = 0;
        }

        public void RecordAce()
        {
            Aces++;
        }

        public void RecordDoubleFault()
        {
            DoubleFaults++;
        }

        public void WinGame()
        {
            GamesWon++;
        }

        public void WinSet()
        {
            SetsWon++;
        }

        public void ResetMatchStats()
        {
            Aces = 0;
            DoubleFaults = 0;
            GamesWon = 0;
            SetsWon = 0;
        }
    }
}