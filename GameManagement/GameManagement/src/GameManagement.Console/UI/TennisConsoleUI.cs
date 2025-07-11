using System;
using System.Linq;
using GameManagement.Core.Games.Tennis;
using GameManagement.Core.Abstractions;

namespace GameManagement.Console.UI
{
    public class TennisConsoleUI
    {
        private readonly TennisMatch _match;
        private int _currentGameScore1 = 0;
        private int _currentGameScore2 = 0;

        public TennisConsoleUI(TennisMatch match)
        {
            _match = match;
        }

        public void ShowScore()
        {
            System.Console.WriteLine("=== SCORE DU MATCH ===");
            System.Console.WriteLine();
            
            var result = _match.GetResult() as TennisResult;
            
            System.Console.WriteLine($"{_match.Players[0].Name}: {result.SetsWon[_match.Players[0]]} sets");
            System.Console.WriteLine($"{_match.Players[1].Name}: {result.SetsWon[_match.Players[1]]} sets");
            System.Console.WriteLine();
            
            // Simplified game score display
            System.Console.WriteLine("Score du jeu actuel:");
            System.Console.WriteLine($"{_match.Players[0].Name}: {GetTennisScore(_currentGameScore1)}");
            System.Console.WriteLine($"{_match.Players[1].Name}: {GetTennisScore(_currentGameScore2)}");
            System.Console.WriteLine();
        }

        private string GetTennisScore(int points)
        {
            return points switch
            {
                0 => "0",
                1 => "15",
                2 => "30",
                3 => "40",
                _ => "40+"
            };
        }

        public IPlayer GetScoringPlayer()
        {
            System.Console.WriteLine("Qui a marqué le point?");
            System.Console.WriteLine($"1. {_match.Players[0].Name}");
            System.Console.WriteLine($"2. {_match.Players[1].Name}");
            System.Console.Write("Choix: ");
            
            if (int.TryParse(System.Console.ReadLine(), out int choice))
            {
                if (choice == 1)
                {
                    _currentGameScore1++;
                    return _match.Players[0];
                }
                else if (choice == 2)
                {
                    _currentGameScore2++;
                    return _match.Players[1];
                }
            }
            
            return null;
        }

        public void ShowResult()
        {
            var result = _match.GetResult();
            
            System.Console.WriteLine("=== MATCH TERMINÉ ===");
            System.Console.WriteLine();
            System.Console.WriteLine($"{result.Winner.Name} remporte le match!");
            System.Console.WriteLine();
            System.Console.WriteLine("Score final:");
            
            var tennisResult = result as TennisResult;
            foreach (var player in _match.Players)
            {
                System.Console.WriteLine($"{player.Name}: {tennisResult.SetsWon[player]} sets");
            }
        }
    }
}