namespace GameManagement.Core.Abstractions
{
    public interface IGameState
    {
        void ValidateMove();
        void UpdateState();
    }
}