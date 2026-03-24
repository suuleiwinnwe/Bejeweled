using System;

class Program
{
    static void Main()
    {
        GameController game = new GameController(8, 8, 10);

        string[] preset =
        {
            "RBGYPBRG",
            "GYRBPYGB",
            "BPGYRGBY",
            "YGBRPYGR",
            "PBRGYBPG",
            "GRYBPGRY",
            "BYGPRYBP",
            "RPBGYGRP"
        };

        game.StartGame(preset);

        Console.WriteLine("Initial Board:");
        game.GetBoard().PrintBoard();
        game.PrintStatus();

        game.TrySwap(0, 0, 0, 1, true);

        Console.WriteLine("Final Board:");
        game.GetBoard().PrintBoard();
        game.PrintStatus();

        if (game.IsGameOver())
        {
            Console.WriteLine("Game Over.");
        }
    }
}