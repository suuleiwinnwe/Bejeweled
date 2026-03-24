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
        if (preset.Length != rows)
            throw new Exception("Preset row count does not match board size.");

        for (int i = 0; i < rows; i++)
        {
            if (preset[i].Length != cols)
                throw new Exception("Preset column count does not match board size.");

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

    public bool IsInBounds(int r, int c)
    {
        return r >= 0 && r < rows && c >= 0 && c < cols;
    }

    public bool IsAdjacent(int r1, int c1, int r2, int c2)
    {
        int rowDiff = Math.Abs(r1 - r2);
        int colDiff = Math.Abs(c1 - c2);

        return (rowDiff == 1 && colDiff == 0) ||
               (rowDiff == 0 && colDiff == 1);
    }

    public void Swap(int r1, int c1, int r2, int c2)
    {
        GemType temp = grid[r1, c1].Type;
        grid[r1, c1].Type = grid[r2, c2].Type;
        grid[r2, c2].Type = temp;
    }

    public SortedSet<Position> FindMatches()
    {
        SortedSet<Position> matches = new SortedSet<Position>();

        // check rows
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

        // check columns
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

        foreach (Position pos in matches)
        {
            if (!grid[pos.Row, pos.Col].IsEmpty())
            {
                grid[pos.Row, pos.Col].Type = GemType.Empty;
                count++;
            }
        }

        return count;
    }

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
                    {
                        grid[i, j].Type = GemType.Empty;
                    }

                    write--;
                }
            }

            while (write >= 0)
            {
                grid[write, j].Type = GemType.Empty;
                write--;
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

    public bool HasLegalMove()
    {
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (j + 1 < cols)
                {
                    Swap(i, j, i, j + 1);
                    bool hasMatch = FindMatches().Count > 0;
                    Swap(i, j, i, j + 1);

                    if (hasMatch)
                        return true;
                }

                if (i + 1 < rows)
                {
                    Swap(i, j, i + 1, j);
                    bool hasMatch = FindMatches().Count > 0;
                    Swap(i, j, i + 1, j);

                    if (hasMatch)
                        return true;
                }
            }
        }

        return false;
    }
}