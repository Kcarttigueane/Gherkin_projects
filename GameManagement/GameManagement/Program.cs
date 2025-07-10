using System;

using GameManagement.Console.UI;
using GameManagement.Core.Common;
using GameManagement.Core.Games.TicTacToe;

namespace GameManagement.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameManager = new GameManager();
            gameManager.Run();
        }
    }

    public class GameManager
    {
        private readonly IGameUI _ui;

        public GameManager()
        {
            _ui = new ConsoleGameUi();
        }

        public void Run()
        {
            while (true)
            {
                _ui.Clear();
                _ui.ShowMainMenu();
                
                var choice = _ui.GetMenuChoice();
                
                switch (choice)
                {
                    case 1:
                        PlayTicTacToe();
                        break;
                    case 4:
                        return;
                    default:
                        _ui.ShowError("Choix invalide");
                        break;
                }
                
                _ui.WaitForKey();
            }
        }

        private void PlayTicTacToe()
        {
            var game = new TicTacToeGame();
            var ticTacToeUI = new TicTacToeConsoleUI(game);
            
            // Setup players
            _ui.ShowMessage("=== Tic Tac Toe ===");
            
            var player1Name = _ui.GetPlayerName(1);
            var player1 = new TicTacToePlayer(player1Name, 'X');
            game.AddPlayer(player1);
            
            var player2Name = _ui.GetPlayerName(2);
            var player2 = new TicTacToePlayer(player2Name, 'O');
            game.AddPlayer(player2);
            
            game.StartGame();
            
            // Game loop
            while (!game.IsGameOver())
            {
                _ui.Clear();
                ticTacToeUI.ShowBoard();
                _ui.ShowMessage($"Tour de {game.CurrentPlayer.Name}");
                
                try
                {
                    var (row, col) = ticTacToeUI.GetMove();
                    game.MakeMove(row, col);
                }
                catch (Exception ex)
                {
                    _ui.ShowError(ex.Message);
                    _ui.WaitForKey();
                }
            }
            
            // Show result
            _ui.Clear();
            ticTacToeUI.ShowBoard();
            ticTacToeUI.ShowResult();
        }
    }
}