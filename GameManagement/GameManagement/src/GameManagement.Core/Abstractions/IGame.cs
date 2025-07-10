using System;
using System.Collections.Generic;
using GameManagement.Core.Common;

namespace GameManagement.Core.Abstractions
{
    public interface IGame
    {
        Guid Id { get; }
        string Name { get; }
        GameStatus Status { get; }
        IReadOnlyList<IPlayer> Players { get; }
        IPlayer CurrentPlayer { get; }
        
        void AddPlayer(IPlayer player);
        void StartGame();
        void EndTurn();
        IGameResult GetResult();
        bool IsGameOver();
    }
}