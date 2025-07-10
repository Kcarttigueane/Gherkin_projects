using System;
using GameManagement.Core.Games.TicTacToe;

namespace GameManagement.Console.UI
{
    public class TicTacToeConsoleUI
    {
        private readonly TicTacToeGame _game;
        private readonly char[,] _displayBoard;

        public TicTacToeConsoleUI(TicTacToeGame game)
        {
            _game = game;
            _displayBoard = new char[3, 3];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _displayBoard[i, j] = ' ';
                }
            }
        }

        public void ShowBoard()
        {
            System.Console.WriteLine("    0   1   2");
            System.Console.WriteLine("  +---+---+---+");
            for (int i = 0; i < 3; i++)
            {
                System.Console.Write($"{i} |");
                for (int j = 0; j < 3; j++)
                {
                    System.Console.Write($" {_displayBoard[i, j]} |");
                }

                System.Console.WriteLine();
                System.Console.WriteLine("  +---+---+---+");
            }

            System.Console.WriteLine();
        }

        public (int row, int col) GetMove()
        {
            System.Console.Write("Ligne (0-2): ");
            if (!int.TryParse(System.Console.ReadLine(), out int row) || row < 0 || row > 2)
            {
                throw new ArgumentException("Ligne invalide");
            }

            System.Console.Write("Colonne (0-2): ");
            if (!int.TryParse(System.Console.ReadLine(), out int col) || col < 0 || col > 2)
            {
                throw new ArgumentException("Colonne invalide");
            }

            // Update display board
            var currentPlayer = _game.CurrentPlayer as TicTacToePlayer;
            _displayBoard[row, col] = currentPlayer.Symbol;
            return (row, col);
        }

        public void ShowResult()
        {
            var result = _game.GetResult();
            System.Console.WriteLine("=== PARTIE TERMINÉE ===");
            System.Console.WriteLine();
            if (result.IsDraw)
            {
                System.Console.WriteLine("Match nul!");
            }
            else
            {
                System.Console.WriteLine($"{result.Winner.Name} a gagné!");
            }
        }
    }
}