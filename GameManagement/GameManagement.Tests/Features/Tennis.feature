Feature: Tennis Match Management
    As a game library user
    I want to manage tennis matches
    So that players can play according to official tennis scoring rules

Background:
    Given a new tennis match
    And player "Federer"
    And player "Nadal"

Scenario: Start a tennis match
    When the match is started
    Then the match status should be "InProgress"
    And both players should have 0 sets won

Scenario: Score points in a game
    Given the match has started
    When "Federer" scores a point
    Then the game score should be "15-0"
    When "Nadal" scores a point
    Then the game score should be "15-15"
    When "Federer" scores a point
    Then the game score should be "30-15"
    When "Federer" scores a point
    Then the game score should be "40-15"

Scenario: Win a game
    Given the match has started
    When "Federer" scores 4 consecutive points
    Then "Federer" should win the current game
    And the games score should be "1-0"

Scenario: Deuce and advantage
    Given the match has started
    And the game score is "40-40"
    When "Federer" scores a point
    Then the game score should be "AD-40"
    When "Nadal" scores a point
    Then the game score should be "40-40"
    When "Nadal" scores a point
    Then the game score should be "40-AD"
    When "Nadal" scores a point
    Then "Nadal" should win the current game

Scenario: Win a set with 6 games
    Given the match has started
    And "Federer" has won 5 games in the current set
    And "Nadal" has won 3 games in the current set
    When "Federer" wins another game
    Then "Federer" should win the set
    And the set score should be "6-3"

Scenario: Must win set by 2 games
    Given the match has started
    And the games score is "5-5"
    When "Federer" wins a game
    Then the set should not be complete
    When "Nadal" wins a game
    Then the games score should be "6-6"
    When "Federer" wins a game
    And "Federer" wins another game
    Then "Federer" should win the set "8-6"

Scenario: Win match with 2 sets
    Given the match has started
    And "Federer" has won 1 set
    When "Federer" wins another set
    Then "Federer" should win the match
    And the match should be over

Scenario: Three set match
    Given the match has started
    When "Federer" wins the first set
    And "Nadal" wins the second set
    And "Federer" wins the third set
    Then "Federer" should win the match
    And the final score should be "2-1"

Scenario: Cannot score point in completed match
    Given the match has started
    And "Federer" has won the match
    When trying to score a point for "Nadal"
    Then an error "Match is not in progress" should be thrown

Scenario: Cannot start match without exactly 2 players
    Given a new tennis match
    And player "Federer"
    When trying to start the match
    Then an error "Tennis requires exactly 2 players" should be thrown

Scenario: Cannot add more than 2 players
    Given player "Djokovic"
    When trying to add "Djokovic" to the match
    Then an error "Tennis can only have 2 players" should be thrown

Scenario Outline: Game scoring progression
    Given the match has started
    When "<player>" scores <points> points
    Then the game score should be "<score>"

    Examples:
        | player   | points | score  |
        | Federer  | 1      | 15-0   |
        | Federer  | 2      | 30-0   |
        | Federer  | 3      | 40-0   |
        | Nadal    | 1      | 0-15   |
        | Nadal    | 2      | 0-30   |
        | Nadal    | 3      | 0-40   |