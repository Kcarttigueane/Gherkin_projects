using System;
using System.Collections.Generic;
using System.Linq;
using GameManagement.Core.Abstractions;

namespace GameManagement.Core.Common
{
    public abstract class BaseGame : IGame
    {
        protected readonly List<IPlayer> _players = new();
        protected int _currentPlayerIndex = 0;

        public Guid Id { get; }
        public abstract string Name { get; }
        public GameStatus Status { get; protected set; }
        public IReadOnlyList<IPlayer> Players => _players.AsReadOnly();
        
        public IPlayer CurrentPlayer => Status == GameStatus.InProgress && _players.Any() 
            ? _players[_currentPlayerIndex] 
            : null;

        protected BaseGame()
        {
            Id = Guid.NewGuid();
            Status = GameStatus.NotStarted;
        }

        public virtual void AddPlayer(IPlayer player)
        {
            if (Status != GameStatus.NotStarted)
                throw new InvalidOperationException("Cannot add players after game has started");
            
            if (player == null)
                throw new ArgumentNullException(nameof(player));
            
            if (_players.Any(p => p.Id == player.Id))
                throw new InvalidOperationException("Player already added to game");
            
            _players.Add(player);
        }

        public virtual void StartGame()
        {
            if (Status != GameStatus.NotStarted)
                throw new InvalidOperationException("Game has already started");
            
            ValidateGameStart();
            Status = GameStatus.InProgress;
        }

        public virtual void EndTurn()
        {
            if (Status != GameStatus.InProgress)
                throw new InvalidOperationException("Game is not in progress");
            
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            
            if (IsGameOver())
            {
                Status = GameStatus.Completed;
            }
        }

        public abstract IGameResult GetResult();
        public abstract bool IsGameOver();
        protected abstract void ValidateGameStart();
    }
}