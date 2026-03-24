using System;
using System.Collections.Generic;

public class Board
{
    private int rows;
    private int cols;
    private Gem[,] grid;
    private Random rand = new Random();

    public Board(int r, int c)
    {
        rows = r;
        cols = c;
        grid = new Gem[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = new Gem();
            }
        }

        FillRandom();
    }

    private GemType RandomGem()
    {
        return (GemType)rand.Next(1, 6);
    }

    public void FillRandom()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j].Type = RandomGem();
            }
        }
    }

    public void LoadPreset(string[] preset)
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                char ch = preset[i][j];

                if (ch == 'R') grid[i, j].Type = GemType.Red;
                else if (ch == 'B') grid[i, j].Type = GemType.Blue;
                else if (ch == 'G') grid[i, j].Type = GemType.Green;
                else if (ch == 'Y') grid[i, j].Type = GemType.Yellow;
                else if (ch == 'P') grid[i, j].Type = GemType.Purple;
                else grid[i, j].Type = GemType.Empty;
            }
        }
    }

    public void PrintBoard()
    {
        Console.WriteLine();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(grid[i, j].ToChar() + " ");
            }
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    // -------- MATCH DETECTION --------
    public SortedSet<Position> FindMatches()
    {
        SortedSet<Position> matches = new SortedSet<Position>();

        // rows
        for (int i = 0; i < rows; i++)
        {
            int start = 0;

            while (start < cols)
            {
                int end = start + 1;

                while (end < cols && grid[i, start].Type == grid[i, end].Type)
                {
                    end++;
                }

                if (end - start >= 3 && !grid[i, start].IsEmpty())
                {
                    for (int j = start; j < end; j++)
                    {
                        matches.Add(new Position(i, j));
                    }
                }

                start = end;
            }
        }

        // columns
        for (int j = 0; j < cols; j++)
        {
            int start = 0;

            while (start < rows)
            {
                int end = start + 1;

                while (end < rows && grid[start, j].Type == grid[end, j].Type)
                {
                    end++;
                }

                if (end - start >= 3 && !grid[start, j].IsEmpty())
                {
                    for (int i = start; i < end; i++)
                    {
                        matches.Add(new Position(i, j));
                    }
                }

                start = end;
            }
        }

        return matches;
    }

    public int ClearMatches(SortedSet<Position> matches)
    {
        int count = 0;

        foreach (var pos in matches)
        {
            if (!grid[pos.Row, pos.Col].IsEmpty())
            {
                grid[pos.Row, pos.Col].Type = GemType.Empty;
                count++;
            }
        }

        return count;
    }

    // GRAVITY 
    public void ApplyGravity()
    {
        for (int j = 0; j < cols; j++)
        {
            int write = rows - 1;

            for (int i = rows - 1; i >= 0; i--)
            {
                if (!grid[i, j].IsEmpty())
                {
                    grid[write, j].Type = grid[i, j].Type;

                    if (write != i)
                        grid[i, j].Type = GemType.Empty;

                    write--;
                }
            }
        }
    }

    public void Refill()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j].IsEmpty())
                {
                    grid[i, j].Type = RandomGem();
                }
            }
        }
    }

    // CASCADE LOOP
    public void ResolveCascades(bool show)
    {
        while (true)
        {
            var matches = FindMatches();

            if (matches.Count == 0)
            {
                Console.WriteLine("No more matches.");
                break;
            }

            Console.WriteLine("Match Found!");
            ClearMatches(matches);
            PrintBoard();

            ApplyGravity();
            Console.WriteLine("After Gravity:");
            PrintBoard();

            Refill();
            Console.WriteLine("After Refill:");
            PrintBoard();
        }
    }
}