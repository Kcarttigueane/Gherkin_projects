Feature: Tic Tac Toe Game Management
    As a game library user
    I want to manage Tic Tac Toe games
    So that players can play according to the official rules

Background:
    Given a new Tic Tac Toe game
    And player "Alice" with symbol "X"
    And player "Bob" with symbol "O"

Scenario: Start a game with two players
    When the game is started
    Then the game status should be "InProgress"
    And the current player should be "Alice"

Scenario: Players alternate turns
    Given the game has started
    When "Alice" places symbol at position (0, 0)
    Then the current player should be "Bob"
    When "Bob" places symbol at position (1, 1)
    Then the current player should be "Alice"

Scenario: Win by completing a row
    Given the game has started
    When the following moves are made:
        | Player | Row | Column |
        | Alice  | 0   | 0      |
        | Bob    | 1   | 0      |
        | Alice  | 0   | 1      |
        | Bob    | 1   | 1      |
        | Alice  | 0   | 2      |
    Then the game should be over
    And "Alice" should be the winner

Scenario: Win by completing a column
    Given the game has started
    When the following moves are made:
        | Player | Row | Column |
        | Alice  | 0   | 0      |
        | Bob    | 0   | 1      |
        | Alice  | 1   | 0      |
        | Bob    | 1   | 1      |
        | Alice  | 2   | 0      |
    Then the game should be over
    And "Alice" should be the winner

Scenario: Win by completing a diagonal
    Given the game has started
    When the following moves are made:
        | Player | Row | Column |
        | Alice  | 0   | 0      |
        | Bob    | 0   | 1      |
        | Alice  | 1   | 1      |
        | Bob    | 0   | 2      |
        | Alice  | 2   | 2      |
    Then the game should be over
    And "Alice" should be the winner

Scenario: Game ends in a draw
    Given the game has started
    When the following moves are made:
        | Player | Row | Column |
        | Alice  | 0   | 0      |
        | Bob    | 0   | 1      |
        | Alice  | 0   | 2      |
        | Bob    | 1   | 1      |
        | Alice  | 1   | 0      |
        | Bob    | 1   | 2      |
        | Alice  | 2   | 1      |
        | Bob    | 2   | 0      |
        | Alice  | 2   | 2      |
    Then the game should be over
    And the game should be a draw

Scenario: Cannot place symbol on occupied position
    Given the game has started
    And "Alice" has placed symbol at position (0, 0)
    When "Bob" tries to place symbol at position (0, 0)
    Then an error "Position already occupied" should be thrown
    And the current player should still be "Bob"

Scenario: Cannot make move in completed game
    Given the game has started
    And "Alice" has won the tic tac toe game
    When "Bob" tries to place symbol at position (2, 2)
    Then an error "Game is not in progress" should be thrown

Scenario: Cannot start game without exactly 2 players
    Given a new Tic Tac Toe game
    And player "Alice" with symbol "X"
    When trying to start the game
    Then an error "Tic Tac Toe requires exactly 2 players" should be thrown

Scenario: Cannot add more than 2 players
    When the game is started
    And player "Charlie" with symbol "C"
    When trying to add "Charlie" to the game
    Then an error "Tic Tac Toe can only have 2 players" should be thrown

Scenario: Invalid board position
    Given the game has started
    When "Alice" tries to place symbol at position (3, 0)
    Then an error "Invalid position" should be thrown