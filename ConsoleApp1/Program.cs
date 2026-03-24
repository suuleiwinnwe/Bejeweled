using System;

class Program
{
    static void Main()
    {
        Board board = new Board(6, 6);

        string[] preset =
        {
            "RRGGGB",
            "BRGGGY",
            "BRPPPY",
            "YBGGGR",
            "YBRRRP",
            "PBYGGR"
        };

        board.LoadPreset(preset);

        Console.WriteLine("Initial Board:");
        board.PrintBoard();

        board.ResolveCascades(true);
    }
}