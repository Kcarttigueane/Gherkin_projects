using System;
using System.Collections.Generic;
using System.Linq;
using GameManagement.Core.Abstractions;
using GameManagement.Core.Common;

namespace GameManagement.Core.Games.Tennis
{
    public class TennisMatch : BaseGame
    {
        private readonly List<TennisSet> _sets = new();
        private TennisSet _currentSet;
        private readonly Dictionary<IPlayer, int> _setsWon = new();

        public override string Name => "Tennis Match";
        public TennisSet CurrentSet => _currentSet;
        public Dictionary<IPlayer, int> SetsWon => new Dictionary<IPlayer, int>(_setsWon);
        public IReadOnlyList<TennisSet> CompletedSets => _sets.AsReadOnly();
        
        public TennisMatch()
        {
        }

        public void ScorePoint(IPlayer scoringPlayer)
        {
            if (Status != GameStatus.InProgress)
                throw new InvalidOperationException("Match is not in progress");

            if (_currentSet == null)
                throw new InvalidOperationException("Current set is not initialized");

            _currentSet.ScorePoint(scoringPlayer);

            if (_currentSet.IsSetComplete())
            {
                var setWinner = _currentSet.GetSetWinner();
                _setsWon[setWinner] = _setsWon.GetValueOrDefault(setWinner) + 1;
                
                // Add the completed set to the list
                if (!_sets.Contains(_currentSet))
                {
                    _sets.Add(_currentSet);
                }

                if (IsGameOver())
                {
                    Status = GameStatus.Completed;
                }
                else
                {
                    _currentSet = new TennisSet(_players[0], _players[1]);
                }
            }
        }

        public override void StartGame()
        {
            base.StartGame();
            
            foreach (var player in _players)
            {
                _setsWon[player] = 0;
            }
            
            _currentSet = new TennisSet(_players[0], _players[1]);
            _sets.Add(_currentSet);
        }

        public override void AddPlayer(IPlayer player)
        {
            if (_players.Count >= 2)
                throw new InvalidOperationException("Tennis can only have 2 players");
            
            base.AddPlayer(player);
        }

        public override bool IsGameOver()
        {
            return _setsWon.Any(kvp => kvp.Value >= 2);
        }

        public override IGameResult GetResult()
        {
            var winner = _setsWon.FirstOrDefault(kvp => kvp.Value >= 2).Key;
            
            return new TennisResult
            {
                Winner = winner,
                IsDraw = false,
                Ranking = _players.OrderByDescending(p => _setsWon.GetValueOrDefault(p)).ToList(),
                SetsWon = new Dictionary<IPlayer, int>(_setsWon)
            };
        }

        protected override void ValidateGameStart()
        {
            if (_players.Count != 2)
                throw new InvalidOperationException("Tennis requires exactly 2 players");
        }
    }

    public class TennisResult : IGameResult
    {
        public bool IsDraw { get; set; }
        public IPlayer Winner { get; set; }
        public IReadOnlyList<IPlayer> Ranking { get; set; }
        public Dictionary<IPlayer, int> SetsWon { get; set; }
        public string Summary => $"{Winner.Name} wins the match!";
    }
}