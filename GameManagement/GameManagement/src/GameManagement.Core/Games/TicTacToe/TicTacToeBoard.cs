using System;

namespace GameManagement.Core.Games.TicTacToe
{
    public class TicTacToeBoard
    {
        private readonly char[,] _grid = new char[3, 3];

        public TicTacToeBoard()
        {
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _grid[i, j] = ' ';
                }
            }
        }

        public void PlaceSymbol(int row, int column, char symbol)
        {
            if (row < 0 || row > 2 || column < 0 || column > 2)
                throw new InvalidOperationException("Invalid position");
            
            if (_grid[row, column] != ' ')
                throw new InvalidOperationException("Position already occupied");
            
            _grid[row, column] = symbol;
        }

        public bool CheckWin(char symbol)
        {
            // Check rows
            for (int i = 0; i < 3; i++)
            {
                if (_grid[i, 0] == symbol && _grid[i, 1] == symbol && _grid[i, 2] == symbol)
                    return true;
            }

            // Check columns
            for (int j = 0; j < 3; j++)
            {
                if (_grid[0, j] == symbol && _grid[1, j] == symbol && _grid[2, j] == symbol)
                    return true;
            }

            // Check diagonals
            if (_grid[0, 0] == symbol && _grid[1, 1] == symbol && _grid[2, 2] == symbol)
                return true;
            
            if (_grid[0, 2] == symbol && _grid[1, 1] == symbol && _grid[2, 0] == symbol)
                return true;

            return false;
        }

        public bool IsFull()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_grid[i, j] == ' ')
                        return false;
                }
            }
            return true;
        }
    }
}