using System;
using System.Linq;
using GameManagement.Core.Abstractions;
using GameManagement.Core.Common;

namespace GameManagement.Core.Games.TicTacToe
{
    public class TicTacToeGame : BaseGame
    {
        private readonly TicTacToeBoard _board;
        private TicTacToePlayer _winner;

        public override string Name => "Tic Tac Toe";

        public TicTacToeGame()
        {
            _board = new TicTacToeBoard();
        }

        public void MakeMove(int row, int column)
        {
            if (Status != GameStatus.InProgress)
                throw new InvalidOperationException("Game is not in progress");

            var currentPlayer = CurrentPlayer as TicTacToePlayer;
            _board.PlaceSymbol(row, column, currentPlayer.Symbol);
            
            if (_board.CheckWin(currentPlayer.Symbol))
            {
                _winner = currentPlayer;
                Status = GameStatus.Completed;
            }
            else if (_board.IsFull())
            {
                Status = GameStatus.Completed;
            }
            else
            {
                EndTurn();
            }
        }

        public override void AddPlayer(IPlayer player)
        {
            if (_players.Count >= 2)
                throw new InvalidOperationException("Tic Tac Toe can only have 2 players");
            
            base.AddPlayer(player);
        }

        public override bool IsGameOver()
        {
            return _winner != null || _board.IsFull();
        }

        public override IGameResult GetResult()
        {
            return new TicTacToeResult
            {
                Winner = _winner,
                IsDraw = _winner == null && _board.IsFull(),
                Ranking = _players.OrderByDescending(p => p == _winner ? 1 : 0).ToList()
            };
        }

        protected override void ValidateGameStart()
        {
            if (_players.Count != 2)
                throw new InvalidOperationException("Tic Tac Toe requires exactly 2 players");
        }
    }

    public class TicTacToeResult : IGameResult
    {
        public bool IsDraw { get; set; }
        public IPlayer Winner { get; set; }
        public IReadOnlyList<IPlayer> Ranking { get; set; }
        public string Summary => IsDraw ? "Game ended in a draw" : $"{Winner.Name} wins!";
    }
}