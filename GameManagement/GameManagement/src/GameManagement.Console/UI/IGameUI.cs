namespace GameManagement.Console.UI
{
    public interface IGameUI
    {
        void Clear();
        void ShowMainMenu();
        int GetMenuChoice();
        string GetPlayerName(int playerNumber);
        int GetPlayerCount();
        void ShowMessage(string message);
        void ShowError(string error);
        void WaitForKey();
    }
}