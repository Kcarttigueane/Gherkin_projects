using System.Collections.Generic;

namespace GameManagement.Core.Abstractions
{
    public interface IGameResult
    {
        bool IsDraw { get; }
        IPlayer Winner { get; }
        IReadOnlyList<IPlayer> Ranking { get; }
        string Summary { get; }
    }
}