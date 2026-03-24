using System;

public class GameController
{
    private Board board;
    private int score;
    private int movesLeft;

    public GameController(int rows, int cols, int moves)
    {
        board = new Board(rows, cols);
        score = 0;
        movesLeft = moves;
    }

    public Board GetBoard()
    {
        return board;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetMovesLeft()
    {
        return movesLeft;
    }

    public void StartGame(string[] preset = null)
    {
        if (preset != null)
            board.LoadPreset(preset);
        else
            board.FillRandom();

        score = 0;
    }

    public void PrintStatus()
    {
        Console.WriteLine("Score: " + score);
        Console.WriteLine("Moves Left: " + movesLeft);
    }

    public bool TrySwap(int r1, int c1, int r2, int c2, bool show)
    {
        if (movesLeft <= 0)
        {
            Console.WriteLine("No moves left.");
            return false;
        }

        if (!board.IsInBounds(r1, c1) || !board.IsInBounds(r2, c2))
        {
            Console.WriteLine("Invalid position.");
            return false;
        }

        if (!board.IsAdjacent(r1, c1, r2, c2))
        {
            Console.WriteLine("Gems are not adjacent.");
            return false;
        }

        board.Swap(r1, c1, r2, c2);

        var matches = board.FindMatches();

        if (matches.Count == 0)
        {
            board.Swap(r1, c1, r2, c2);
            Console.WriteLine("No match. Swap reverted.");
            return false;
        }

        movesLeft--;

        while (matches.Count > 0)
        {
            Console.WriteLine("Match Found!");

            int cleared = board.ClearMatches(matches);
            score += cleared * 10;

            if (show)
            {
                board.PrintBoard();
            }

            board.ApplyGravity();

            if (show)
            {
                Console.WriteLine("After Gravity:");
                board.PrintBoard();
            }

            board.Refill();

            if (show)
            {
                Console.WriteLine("After Refill:");
                board.PrintBoard();
            }

            matches = board.FindMatches();
        }

        return true;
    }

    public bool IsGameOver()
    {
        return movesLeft <= 0 || !board.HasLegalMove();
    }
}