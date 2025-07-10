using TechTalk.SpecFlow;
using FluentAssertions;
using GameManagement.Core.Abstractions;
using GameManagement.Core.Games.TicTacToe;

namespace GameManagement.Tests.StepDefinitions
{
    [Binding]
    public class TicTacToeSteps
    {
        private readonly ScenarioContext _scenarioContext;

        public TicTacToeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"a new Tic Tac Toe game")]
        public void GivenANewTicTacToeGame()
        {
            var game = new TicTacToeGame();
            _scenarioContext["game"] = game;
            _scenarioContext["players"] = new List<IPlayer>();
        }

        [Given(@"player ""([^""]+)"" with symbol ""(.)""")]
        [When(@"player ""([^""]+)"" with symbol ""(.)""")]
        public void GivenPlayerWithSymbol(string playerName, char symbol)
        {
            var player = new TicTacToePlayer(playerName, symbol);
            
            if (!_scenarioContext.ContainsKey("players"))
            {
                _scenarioContext["players"] = new List<IPlayer>();
            }
            
            var players = _scenarioContext.Get<List<IPlayer>>("players");
            players.Add(player);
            
            // Store player reference for easy access
            _scenarioContext[$"player_{playerName}"] = player;
        }

        [When(@"""(.*)"" places symbol at position \((\d+), (\d+)\)")]
        public void WhenPlayerPlacesSymbolAtPosition(string playerName, int row, int column)
        {
            try
            {
                var game = _scenarioContext.Get<TicTacToeGame>("game");
                game.MakeMove(row, column);
            }
            catch (Exception ex)
            {
                _scenarioContext["lastException"] = ex;
            }
        }

        [When(@"""(.*)"" tries to place symbol at position \((\d+), (\d+)\)")]
        public void WhenPlayerTriesToPlaceSymbolAtPosition(string playerName, int row, int column)
        {
            WhenPlayerPlacesSymbolAtPosition(playerName, row, column);
        }

        [When(@"the following moves are made:")]
        public void WhenTheFollowingMovesAreMade(Table table)
        {
            var game = _scenarioContext.Get<TicTacToeGame>("game");
            
            foreach (var row in table.Rows)
            {
                var playerName = row["Player"];
                var boardRow = int.Parse(row["Row"]);
                var boardColumn = int.Parse(row["Column"]);
                
                game.MakeMove(boardRow, boardColumn);
            }
        }

        [Then(@"""(.*)"" should be the winner")]
        public void ThenPlayerShouldBeTheWinner(string playerName)
        {
            var game = _scenarioContext.Get<TicTacToeGame>("game");
            var result = game.GetResult();
            
            result.Winner.Should().NotBeNull();
            result.Winner.Name.Should().Be(playerName);
            result.IsDraw.Should().BeFalse();
        }

        [Then(@"the game should be a draw")]
        public void ThenTheGameShouldBeADraw()
        {
            var game = _scenarioContext.Get<TicTacToeGame>("game");
            var result = game.GetResult();
            
            result.IsDraw.Should().BeTrue();
            result.Winner.Should().BeNull();
        }

        [Given(@"""(.*)"" has placed symbol at position \((\d+), (\d+)\)")]
        public void GivenPlayerHasPlacedSymbolAtPosition(string playerName, int row, int column)
        {
            var game = _scenarioContext.Get<TicTacToeGame>("game");
            game.MakeMove(row, column);
        }

        [Given(@"""(.*)"" has won the game")]
        public void GivenPlayerHasWonTheGame(string playerName)
        {
            var game = _scenarioContext.Get<TicTacToeGame>("game");
            
            // Simulate a winning sequence for the player
            game.MakeMove(0, 0); // Player 1
            game.MakeMove(1, 0); // Player 2
            game.MakeMove(0, 1); // Player 1
            game.MakeMove(1, 1); // Player 2
            game.MakeMove(0, 2); // Player 1 wins
        }

        [Then(@"the current player should still be ""(.*)""")]
        public void ThenTheCurrentPlayerShouldStillBe(string playerName)
        {
            var game = _scenarioContext.Get<TicTacToeGame>("game");
            game.CurrentPlayer.Name.Should().Be(playerName);
        }

        [When(@"trying to add ""(.*)"" to the game")]
        public void WhenTryingToAddPlayerToTheGame(string playerName)
        {
            try
            {
                var game = _scenarioContext.Get<TicTacToeGame>("game");
                var player = _scenarioContext.Get<TicTacToePlayer>($"player_{playerName}");
                game.AddPlayer(player);
            }
            catch (Exception ex)
            {
                _scenarioContext["lastException"] = ex;
            }
        }
    }
}