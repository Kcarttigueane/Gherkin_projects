using System;

namespace GameManagement.Console.UI
{
    public class ConsoleGameUi : IGameUI
    {
        public void Clear()
        {
            System.Console.Clear();
        }

        public void ShowMainMenu()
        {
            System.Console.WriteLine("=================================");
            System.Console.WriteLine("    GAME MANAGEMENT LIBRARY");
            System.Console.WriteLine("=================================");
            System.Console.WriteLine();
            System.Console.WriteLine("1. Jouer au Tic Tac Toe");
            System.Console.WriteLine("2. Jouer au Tennis");
            System.Console.WriteLine("3. Jouer aux Fléchettes (501)");
            System.Console.WriteLine("4. Quitter");
            System.Console.WriteLine();
            System.Console.Write("Votre choix: ");
        }

        public int GetMenuChoice()
        {
            if (int.TryParse(System.Console.ReadLine(), out int choice))
            {
                return choice;
            }
            return -1;
        }

        public string GetPlayerName(int playerNumber)
        {
            System.Console.Write($"Nom du joueur {playerNumber}: ");
            var name = System.Console.ReadLine();
            return string.IsNullOrWhiteSpace(name) ? $"Joueur {playerNumber}" : name;
        }

        public int GetPlayerCount()
        {
            System.Console.Write("Nombre de joueurs: ");
            if (int.TryParse(System.Console.ReadLine(), out int count))
            {
                return count;
            }
            return 2;
        }

        public void ShowMessage(string message)
        {
            System.Console.WriteLine(message);
        }

        public void ShowError(string error)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"Erreur: {error}");
            System.Console.ResetColor();
        }

        public void WaitForKey()
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Appuyez sur une touche pour continuer...");
            System.Console.ReadKey();
        }
    }
}