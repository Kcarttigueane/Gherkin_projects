using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using GameManagement.Core.Games.Tennis;
using GameManagement.Core.Abstractions;
using GameManagement.Core.Common;
using FluentAssertions;

namespace GameManagement.Tests.StepDefinitions
{
    [Binding]
    public class TennisSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public TennisSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"a new tennis match")]
        public void GivenANewTennisMatch()
        {
            var match = new TennisMatch();
            _scenarioContext["game"] = match;
            _scenarioContext["players"] = new List<IPlayer>();
        }

        [Then(@"both players should have (\d+) sets won")]
        public void ThenBothPlayersShouldHaveSetsWon(int expectedSets)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            var result = match.GetResult() as TennisResult;
            
            foreach (var player in match.Players)
            {
                result.SetsWon[player].Should().Be(expectedSets);
            }
        }

        [When(@"""(.*)"" scores a point")]
        public void WhenPlayerScoresAPoint(string playerName)
        {
            try
            {
                var match = _scenarioContext.Get<TennisMatch>("game");
                var player = match.Players.First(p => p.Name == playerName);
                match.ScorePoint(player);
            }
            catch (Exception ex)
            {
                _scenarioContext["lastException"] = ex;
            }
        }

        [When(@"""(.*)"" scores (\d+) consecutive points")]
        public void WhenPlayerScoresConsecutivePoints(string playerName, int points)
        {
            for (int i = 0; i < points; i++)
            {
                WhenPlayerScoresAPoint(playerName);
            }
        }

        [Then(@"the game score should be ""(.*)""")]
        public void ThenTheGameScoreShouldBe(string expectedScore)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            var actualScore = match.CurrentSet.CurrentGame.GetScoreDisplay();
            actualScore.Should().Be(expectedScore);
        }

        [Then(@"""(.*)"" should win the current game")]
        public void ThenPlayerShouldWinTheCurrentGame(string playerName)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            var player = match.Players.First(p => p.Name == playerName);
            var gamesWon = match.CurrentSet.GamesWon;
            
            // Check that the player has won at least one game
            gamesWon[player].Should().BeGreaterThan(0, $"{playerName} should have won at least one game");
        }

        [Then(@"the games score should be ""(.*)""")]
        public void ThenTheGamesScoreShouldBe(string expectedScore)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            var gamesWon = match.CurrentSet.GamesWon;
            var player1Games = gamesWon[match.Players[0]];
            var player2Games = gamesWon[match.Players[1]];
            var actualScore = $"{player1Games}-{player2Games}";
            actualScore.Should().Be(expectedScore);
        }

        [Given(@"the game score is ""(.*)""")]
        public void GivenTheGameScoreIs(string score)
        {
            // Set up the game to have this score
            var match = _scenarioContext.Get<TennisMatch>("game");
            
            // Parse score and set up appropriate points
            if (score == "40-40")
            {
                // Each player scores 3 points to reach deuce
                for (int i = 0; i < 3; i++)
                {
                    match.ScorePoint(match.Players[0]);
                    match.ScorePoint(match.Players[1]);
                }
            }
        }

        [Given(@"""(.*)"" has won (\d+) games in the current set")]
        public void GivenPlayerHasWonGamesInCurrentSet(string playerName, int games)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            var player = match.Players.First(p => p.Name == playerName);
            
            // Simulate winning the specified number of games
            for (int i = 0; i < games; i++)
            {
                // Win a game by scoring 4 points
                for (int j = 0; j < 4; j++)
                {
                    match.ScorePoint(player);
                }
            }
        }

        [When(@"""(.*)"" wins another game")]
        public void WhenPlayerWinsAnotherGame(string playerName)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            var player = match.Players.First(p => p.Name == playerName);
            
            // Win a game by scoring 4 points
            for (int i = 0; i < 4; i++)
            {
                match.ScorePoint(player);
            }
        }

        [Then(@"""(.*)"" should win the set")]
        public void ThenPlayerShouldWinTheSet(string playerName)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            var setsWon = match.SetsWon;
            var player = match.Players.First(p => p.Name == playerName);
            
            // The player should have won the most recent set
            setsWon[player].Should().BeGreaterThan(0);
        }

        [Then(@"the set score should be ""(.*)""")]
        public void ThenTheSetScoreShouldBe(string expectedScore)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            
            // Check if we have completed sets (meaning a set was just finished)
            Dictionary<IPlayer, int> gamesWon;
            if (match.CompletedSets.Any())
            {
                // Use the last completed set
                gamesWon = match.CompletedSets.Last().GamesWon;
            }
            else
            {
                // Use the current set if no sets have been completed yet
                gamesWon = match.CurrentSet.GamesWon;
            }
            
            var player1Games = gamesWon[match.Players[0]];
            var player2Games = gamesWon[match.Players[1]];
            var actualScore = $"{Math.Max(player1Games, player2Games)}-{Math.Min(player1Games, player2Games)}";
            actualScore.Should().Be(expectedScore);
        }

        [Given(@"""(.*)"" has won (\d+) set")]
        [Given(@"""(.*)"" has won (\d+) sets")]
        public void GivenPlayerHasWonSets(string playerName, int sets)
        {
            // Simulate winning the specified number of sets
            var match = _scenarioContext.Get<TennisMatch>("game");
            var player = match.Players.First(p => p.Name == playerName);
            var opponent = match.Players.First(p => p.Name != playerName);
            
            for (int setNum = 0; setNum < sets; setNum++)
            {
                // Win 6 games to win a set (6-0 or 6-4 for speed)
                for (int game = 0; game < 6; game++)
                {
                    // Win each game with 4 points
                    for (int point = 0; point < 4; point++)
                    {
                        match.ScorePoint(player);
                    }
                }
                
                // Add some games for opponent to make it realistic (4 games)
                if (setNum < sets - 1) // Don't add games on last set to avoid complications
                {
                    for (int game = 0; game < 4; game++)
                    {
                        for (int point = 0; point < 4; point++)
                        {
                            match.ScorePoint(opponent);
                        }
                    }
                }
            }
        }

        [When(@"""(.*)"" wins another set")]
        public void WhenPlayerWinsAnotherSet(string playerName)
        {
            GivenPlayerHasWonSets(playerName, 1);
        }

        [Then(@"""(.*)"" should win the match")]
        public void ThenPlayerShouldWinTheMatch(string playerName)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            var result = match.GetResult();
            
            result.Winner.Should().NotBeNull();
            result.Winner.Name.Should().Be(playerName);
        }

        [When(@"""(.*)"" wins the first set")]
        [When(@"""(.*)"" wins the second set")]
        [When(@"""(.*)"" wins the third set")]
        public void WhenPlayerWinsTheSet(string playerName)
        {
            WhenPlayerWinsAnotherSet(playerName);
        }

        [Then(@"the final score should be ""(.*)""")]
        public void ThenTheFinalScoreShouldBe(string expectedScore)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            var setsWon = match.SetsWon;
            
            var player1Sets = setsWon[match.Players[0]];
            var player2Sets = setsWon[match.Players[1]];
            
            var actualScore = $"{Math.Max(player1Sets, player2Sets)}-{Math.Min(player1Sets, player2Sets)}";
            actualScore.Should().Be(expectedScore);
        }

        [Given(@"""(.*)"" has won the match")]
        public void GivenPlayerHasWonTheMatch(string playerName)
        {
            GivenPlayerHasWonSets(playerName, 2);
        }

        [When(@"trying to score a point for ""(.*)""")]
        public void WhenTryingToScorePointFor(string playerName)
        {
            WhenPlayerScoresAPoint(playerName);
        }

        [Then(@"the set should not be complete")]
        public void ThenTheSetShouldNotBeComplete()
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            match.CurrentSet.IsSetComplete().Should().BeFalse();
        }

        [Given(@"the games score is ""(.*)""")]
        public void GivenTheGamesScoreIs(string score)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            var scores = score.Split('-');
            var player1Games = int.Parse(scores[0]);
            var player2Games = int.Parse(scores[1]);
            
            // Simulate winning the specified number of games for each player
            for (int i = 0; i < player1Games; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    match.ScorePoint(match.Players[0]);
                }
            }
            
            for (int i = 0; i < player2Games; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    match.ScorePoint(match.Players[1]);
                }
            }
        }

        [When(@"""(.*)"" wins a game")]
        public void WhenPlayerWinsAGame(string playerName)
        {
            WhenPlayerWinsAnotherGame(playerName);
        }

        [Then(@"""(.*)"" should win the set ""(.*)""")]
        public void ThenPlayerShouldWinTheSetWithScore(string playerName, string expectedScore)
        {
            var match = _scenarioContext.Get<TennisMatch>("game");
            var player = match.Players.First(p => p.Name == playerName);
            var setsWon = match.SetsWon;
            
            setsWon[player].Should().BeGreaterThan(0);
            
            // Check the last completed set for the score
            Dictionary<IPlayer, int> gamesWon;
            if (match.CompletedSets.Any())
            {
                gamesWon = match.CompletedSets.Last().GamesWon;
            }
            else
            {
                gamesWon = match.CurrentSet.GamesWon;
            }
            
            var player1Games = gamesWon[match.Players[0]];
            var player2Games = gamesWon[match.Players[1]];
            var actualScore = $"{Math.Max(player1Games, player2Games)}-{Math.Min(player1Games, player2Games)}";
            actualScore.Should().Be(expectedScore);
        }

        [When(@"""(.*)"" scores (\d+) points")]
        public void WhenPlayerScoresPoints(string playerName, int points)
        {
            WhenPlayerScoresConsecutivePoints(playerName, points);
        }

        [When(@"trying to add ""(.*)"" to the match")]
        public void WhenTryingToAddPlayerToTheMatch(string playerName)
        {
            try
            {
                var match = _scenarioContext.Get<TennisMatch>("game");
                var players = _scenarioContext.Get<List<IPlayer>>("players");
                
                // First add the existing players to the match if they aren't already
                foreach (var existingPlayer in players.Where(p => !match.Players.Any(mp => mp.Id == p.Id)))
                {
                    match.AddPlayer(existingPlayer);
                }
                
                // Now try to add the new player (this should fail if there are already 2)
                var newPlayer = new GameManagement.Core.Common.Player(playerName);
                match.AddPlayer(newPlayer);
            }
            catch (Exception ex)
            {
                _scenarioContext["lastException"] = ex;
            }
        }
    }
}