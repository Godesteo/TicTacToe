using System;

class TicTacToeAI
{
    private const int BOARD_SIZE = 3;
    private const int X_WIN = 0;
    private const int O_WIN = 1;
    private const int DRAW = 2;
    private const int IN_PROGRESS = -99;

    private char[,] board;

    private bool isValid = true;

    static void Main(string[] args)
    {
        TicTacToeAI game = new TicTacToeAI();
        Console.WriteLine("Welcome to Tic-Tac-Toe!");
        Console.WriteLine("You are playing as X, and the AI is playing as O.");

        while (!game.IsGameOver())
        {
            game.PrintBoard();

            // Player's turn
            Console.WriteLine("Make a move:");
            Console.Write("Row: ");
            int row = int.Parse(Console.ReadLine());
            Console.Write("Column: ");
            int col = int.Parse(Console.ReadLine());

            game.MakeMove(row, col, 'X');

            // AI's turn
            if (game.isValid)
            {
                game.MakeMoveAI('O');
            }

        }

        game.PrintBoard();

        int score = game.Evaluate();

        if (score == X_WIN)
        {
            Console.WriteLine("You won!");
        }
        else if (score == O_WIN)
        {
            Console.WriteLine("You lost!");
        }
        else
        {
            Console.WriteLine("Draw!");
        }

        Console.ReadLine();
    }

    public TicTacToeAI()
    {
        board = new char[BOARD_SIZE, BOARD_SIZE];
        InitBoard();
    }

    private void InitBoard()
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                board[i, j] = '-';
            }
        }
    }

    public void PrintBoard()
    {
        Console.WriteLine("Current board state:");
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                Console.Write(board[i, j] + " ");
            }
            Console.WriteLine();
        }
    }

    public bool IsGameOver()
    {
        return IsWinner('X') || IsWinner('O') || IsBoardFull();
    }

    private bool IsWinner(char player)
    {
        // Check rows
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            if (board[i, 0] == player && board[i, 1] == player && board[i, 2] == player)
            {
                return true;
            }
        }

        // Check columns
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            if (board[0, i] == player && board[1, i] == player && board[2, i] == player)
            {
                return true;
            }
        }

        // Check diagonals
        if (board[0, 0] == player && board[1, 1] == player && board[2, 2] == player)
        {
            return true;
        }

        if (board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
        {
            return true;
        }

        return false;
    }

    private bool IsBoardFull()
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (board[i, j] == '-')
                {
                    return false;
                }
            }
        }

        return true;
    }

    public int Evaluate()
    {
        if (IsWinner('X'))
        {
            return X_WIN;
        }
        else if (IsWinner('O'))
        {
            return O_WIN;
        }
        else if (IsBoardFull())
        {
            return DRAW;
        }
        else
        {
            return IN_PROGRESS;
        }
    }

    public void MakeMove(int row, int col, char player)
    {
        if (board[row, col] != '-' || row > 2 || row < 0 || col > 2 || col < 0)
        {
            Console.WriteLine("Invalid move!");
            isValid = false;
        }
        else
        {
            board[row, col] = player;
            isValid = true;
        }
    }

    public void MakeMoveAI(char player)
    {
        var bestMove = GetBestMove(player);
        if (bestMove == (-1, -1))
        {
            Console.WriteLine("Draw!");
            Console.ReadLine();
        }
        else
        {
            MakeMove(bestMove.Row, bestMove.Col, player);
            Console.WriteLine($"AI ({player}) made a move at row {bestMove.Row} and column {bestMove.Col}.");
        }

    }

    private (int Row, int Col) GetBestMove(char player)
    {
        if (!IsBoardFull())
        {

            (int Row, int Col) bestMove = (-1, -1);

            bestMove = CheckRows(player);
            if (bestMove != (-1, -1))
            {
                return bestMove;
            }

            bestMove = CheckRows(player == 'X' ? 'O' : 'X');
            if (bestMove != (-1, -1))
            {
                return bestMove;
            }

            bestMove = CheckColumns(player);
            if (bestMove != (-1, -1))
            {
                return bestMove;
            }

            bestMove = CheckColumns(player == 'X' ? 'O' : 'X');
            if (bestMove != (-1, -1))
            {
                return bestMove;
            }

            bestMove = CheckMainDiagonal(player);
            if (bestMove != (-1, -1))
            {
                return bestMove;
            }

            bestMove = CheckMainDiagonal(player == 'X' ? 'O' : 'X');
            if (bestMove != (-1, -1))
            {
                return bestMove;
            }

            bestMove = CheckSecondaryDiagonal(player);
            if (bestMove != (-1, -1))
            {
                return bestMove;
            }

            bestMove = CheckSecondaryDiagonal(player == 'X' ? 'O' : 'X');
            if (bestMove != (-1, -1))
            {
                return bestMove;
            }

            return bestMove = GetRandomMove();
        }
        else
        {
            return (-1, -1);
        }
    }

    private (int, int) CheckRows(char player)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            int count = 0;
            int row = -1, col = -1;

            for (int j = 0; j < BOARD_SIZE; j++)
            {
                if (board[i, j] == player)
                {
                    count++;
                }
                else if (board[i, j] != '-')
                {
                    count = -1;
                    break;
                }
                else
                {
                    row = i;
                    col = j;
                }
            }

            if (count == 2)
            {
                return (row, col);
            }
            else
            {
                return (-1, -1);
            }
        }

        return (-1, -1);
    }
    private (int, int) CheckColumns(char player)
    {
        int row = -1;
        int col = -1;

        for (int j = 0; j < BOARD_SIZE; j++)
        {
            int count = 0;
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                if (board[i, j] == player)
                {
                    count++;
                }
                else if (board[i, j] != '-')
                {
                    count = -1;
                    break;
                }
                else
                {
                    row = i;
                    col = j;
                }
            }

            if (count == 2)
            {
                return (row, col);
            }
            else
            {
                return (-1, -1);
            }
        }

        return (-1, -1);
    }
    private (int, int) CheckMainDiagonal(char player)
    {
        int row = -1, col = -1;
        int diagCount = 0;
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            if (board[i, i] == player)
                diagCount++;
            else if (board[i, i] != '-')
            {
                diagCount = -1;
                break;
            }
            else
            {
                row = i;
                col = i;
            }
        }

        if (diagCount == 2)
        {
            return (row, col);
        }
        else
        {
            return (-1, -1);
        }
    }
    private (int, int) CheckSecondaryDiagonal(char player)
    {
        int row = -1;
        int col = -1;
        int diagCount = 0;

        for (int i = 0; i < BOARD_SIZE; i++)
        {
            if (board[i, BOARD_SIZE - 1 - i] == player)
            {
                diagCount++;
            }
            else if (board[i, BOARD_SIZE - 1 - i] != '-')
            {
                diagCount = -1;
                break;
            }
            else
            {
                row = i;
                col = BOARD_SIZE - 1 - i;
            }
        }

        if (diagCount == 2)
        {
            return (row, col);
        }

        return (-1, -1); // return an invalid move if no winning move is found
    }
    private (int, int) GetRandomMove()
    {
        Random rand = new Random();
        int row, col;

        do
        {
            row = rand.Next(0, BOARD_SIZE);
            col = rand.Next(0, BOARD_SIZE);
        } while (board[row, col] != '-');

        return (row, col);
    }
}





