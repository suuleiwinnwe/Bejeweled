using System;

public struct Position : IComparable<Position>
{
    public int Row;
    public int Col;

    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public int CompareTo(Position other)
    {
        if (Row != other.Row)
            return Row.CompareTo(other.Row);

        return Col.CompareTo(other.Col);
    }
}