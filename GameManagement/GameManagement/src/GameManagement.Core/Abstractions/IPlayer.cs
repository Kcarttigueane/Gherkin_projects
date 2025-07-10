using System;

namespace GameManagement.Core.Abstractions
{
    public interface IPlayer
    {
        Guid Id { get; }
        string Name { get; }
        int Score { get; }
    }
}