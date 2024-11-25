using MazeLibrary;
using System;

namespace MazeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var generator = new MazeGenerator();
            var pathfinder = new Pathfinder();
            var fileHandler = new FileHandler();

            int width = 21, height = 21;
            var maze = generator.GenerateMaze(width, height);
            generator.PrintMaze(maze);

            var start = (0, 0);
            var end = (width - 1, height - 1);

            Console.WriteLine("\nDFS Path:");
            var dfsPath = pathfinder.FindPathDFS(maze, start, end);
            generator.PrintMaze(maze, dfsPath);

            Console.WriteLine("\nA* Path:");
            var aStarPath = pathfinder.FindPathAStar(maze, start, end);
            generator.PrintMaze(maze, aStarPath);

            Console.WriteLine("\nDijkstra Path:");
            var dijkstraPath = pathfinder.FindPathDijkstra(maze, start, end);
            generator.PrintMaze(maze, dijkstraPath);
        }
    }
}
