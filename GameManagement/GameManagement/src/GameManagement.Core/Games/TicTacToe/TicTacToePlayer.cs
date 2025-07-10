using GameManagement.Core.Common;

namespace GameManagement.Core.Games.TicTacToe
{
    public class TicTacToePlayer : Player
    {
        public char Symbol { get; }

        public TicTacToePlayer(string name, char symbol) : base(name)
        {
            Symbol = symbol;
        }
    }
}