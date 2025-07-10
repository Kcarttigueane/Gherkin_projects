using System;
using GameManagement.Core.Abstractions;

namespace GameManagement.Core.Common
{
    public class Player : IPlayer
    {
        public Guid Id { get; }
        public string Name { get; }
        public int Score { get; protected set; }

        public Player(string name)
        {
            Id = Guid.NewGuid();
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Score = 0;
        }

        public virtual void UpdateScore(int points)
        {
            Score += points;
        }
    }
}