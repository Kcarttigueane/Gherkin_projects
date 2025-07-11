using System;
using System.Collections.Generic;
using System.Linq;
using GameManagement.Core.Abstractions;

namespace GameManagement.Core.Games.Tennis
{
    public class TennisSet
    {
        private readonly Dictionary<IPlayer, int> _gamesWon = new();
        private TennisGame _currentGame;
        private readonly IPlayer _player1;
        private readonly IPlayer _player2;

        public TennisSet(IPlayer player1, IPlayer player2)
        {
            _player1 = player1;
            _player2 = player2;
            _gamesWon[player1] = 0;
            _gamesWon[player2] = 0;
            _currentGame = new TennisGame(player1, player2);
        }

        public Dictionary<IPlayer, int> GamesWon => new Dictionary<IPlayer, int>(_gamesWon);
        public TennisGame CurrentGame => _currentGame;

        public void ScorePoint(IPlayer scoringPlayer)
        {
            _currentGame.ScorePoint(scoringPlayer);

            if (_currentGame.IsGameComplete())
            {
                var gameWinner = _currentGame.GetGameWinner();
                _gamesWon[gameWinner]++;
                
                // Start a new game if the set is not complete
                if (!IsSetComplete())
                {
                    _currentGame = new TennisGame(_player1, _player2);
                }
            }
        }

        public bool IsSetComplete()
        {
            var player1Games = _gamesWon[_player1];
            var player2Games = _gamesWon[_player2];

            // Win by 2 games with at least 6 games
            return (player1Games >= 6 || player2Games >= 6) && 
                   Math.Abs(player1Games - player2Games) >= 2;
        }

        public IPlayer GetSetWinner()
        {
            if (!IsSetComplete())
                return null;

            return _gamesWon[_player1] > _gamesWon[_player2] ? _player1 : _player2;
        }
    }

    public class TennisGame
    {
        private readonly Dictionary<IPlayer, TennisScore> _scores = new();
        private readonly IPlayer _player1;
        private readonly IPlayer _player2;
        private bool _isDeuce;

        public TennisGame(IPlayer player1, IPlayer player2)
        {
            _player1 = player1;
            _player2 = player2;
            _scores[player1] = TennisScore.Love;
            _scores[player2] = TennisScore.Love;
        }

        public void ScorePoint(IPlayer scoringPlayer)
        {
            var currentScore = _scores[scoringPlayer];
            var opponentScore = _scores[scoringPlayer == _player1 ? _player2 : _player1];

            if (_isDeuce)
            {
                HandleDeuceScoring(scoringPlayer);
            }
            else
            {
                _scores[scoringPlayer] = GetNextScore(currentScore);
                
                if (_scores[_player1] == TennisScore.Forty && _scores[_player2] == TennisScore.Forty)
                {
                    _isDeuce = true;
                }
            }
        }

        private void HandleDeuceScoring(IPlayer scoringPlayer)
        {
            var opponent = scoringPlayer == _player1 ? _player2 : _player1;
            
            if (_scores[scoringPlayer] == TennisScore.Advantage)
            {
                _scores[scoringPlayer] = TennisScore.Game;
            }
            else if (_scores[opponent] == TennisScore.Advantage)
            {
                _scores[opponent] = TennisScore.Forty;
            }
            else
            {
                _scores[scoringPlayer] = TennisScore.Advantage;
            }
        }

        private TennisScore GetNextScore(TennisScore current)
        {
            return current switch
            {
                TennisScore.Love => TennisScore.Fifteen,
                TennisScore.Fifteen => TennisScore.Thirty,
                TennisScore.Thirty => TennisScore.Forty,
                TennisScore.Forty => TennisScore.Game,
                _ => current
            };
        }

        public bool IsGameComplete()
        {
            return _scores.Any(kvp => kvp.Value == TennisScore.Game);
        }

        public IPlayer GetGameWinner()
        {
            return _scores.FirstOrDefault(kvp => kvp.Value == TennisScore.Game).Key;
        }

        public string GetScoreDisplay()
        {
            var player1Score = _scores[_player1];
            var player2Score = _scores[_player2];
            
            if (_isDeuce && player1Score == TennisScore.Forty && player2Score == TennisScore.Forty)
            {
                return "40-40";
            }
            
            return $"{player1Score.ToDisplayString()}-{player2Score.ToDisplayString()}";
        }
    }
}