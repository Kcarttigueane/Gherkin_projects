using System;
using System.Linq;
using TechTalk.SpecFlow;

using FluentAssertions;
using GameManagement.Core.Abstractions;
using GameManagement.Core.Common;

namespace GameManagement.Tests.StepDefinitions
{
    [Binding]
    public class CommonSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public CommonSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"player ""([^""]+)""$")]
        public void GivenPlayer(string playerName)
        {
            var player = new Player(playerName);
            
            if (!_scenarioContext.ContainsKey("players"))
            {
                _scenarioContext["players"] = new List<IPlayer>();
            }
            
            var players = _scenarioContext.Get<List<IPlayer>>("players");
            players.Add(player);
        }

        [When(@"the game is started")]
        [When(@"the match is started")]
        public void WhenTheGameIsStarted()
        {
            try
            {
                var game = _scenarioContext.Get<IGame>("game");
                var players = _scenarioContext.Get<List<IPlayer>>("players");
                
                foreach (var player in players)
                {
                    game.AddPlayer(player);
                }
                
                game.StartGame();
            }
            catch (Exception ex)
            {
                _scenarioContext["lastException"] = ex;
            }
        }

        [When(@"trying to start the game")]
        [When(@"trying to start the match")]
        public void WhenTryingToStartTheGame()
        {
            WhenTheGameIsStarted();
        }

        [Then(@"the game status should be ""(.*)""")]
        [Then(@"the match status should be ""(.*)""")]
        public void ThenTheGameStatusShouldBe(string expectedStatus)
        {
            var game = _scenarioContext.Get<IGame>("game");
            var status = Enum.Parse<GameStatus>(expectedStatus);
            game.Status.Should().Be(status);
        }

        [Then(@"the current player should be ""(.*)""")]
        public void ThenTheCurrentPlayerShouldBe(string playerName)
        {
            var game = _scenarioContext.Get<IGame>("game");
            game.CurrentPlayer.Name.Should().Be(playerName);
        }

        [Then(@"the game should be over")]
        [Then(@"the match should be over")]
        public void ThenTheGameShouldBeOver()
        {
            var game = _scenarioContext.Get<IGame>("game");
            game.IsGameOver().Should().BeTrue();
            game.Status.Should().Be(GameStatus.Completed);
        }

        [Then(@"an error ""(.*)"" should be thrown")]
        public void ThenAnErrorShouldBeThrown(string expectedMessage)
        {
            _scenarioContext.Should().ContainKey("lastException");
            var exception = _scenarioContext.Get<Exception>("lastException");
            exception.Message.Should().Be(expectedMessage);
        }

        [Given(@"the game has started")]
        [Given(@"the match has started")]
        public void GivenTheGameHasStarted()
        {
            WhenTheGameIsStarted();
        }
    }
}