using System;

namespace MazeLibrary
{
    public class MazeGenerator
    {
        public int[,] GenerateMaze(int width, int height)
        {
            var maze = new int[height, width];
            GenerateRecursive(maze, 0, 0, width, height);
            return maze;
        }

        private void GenerateRecursive(int[,] maze, int x, int y, int width, int height)
        {
            maze[y, x] = 1;

            var directions = new (int dx, int dy)[] { (0, -2), (2, 0), (0, 2), (-2, 0) };
            Shuffle(directions);

            foreach (var (dx, dy) in directions)
            {
                int nx = x + dx, ny = y + dy;
                if (IsInBounds(nx, ny, width, height) && maze[ny, nx] == 0)
                {
                    maze[y + dy / 2, x + dx / 2] = 1;
                    GenerateRecursive(maze, nx, ny, width, height);
                }
            }
        }

        private bool IsInBounds(int x, int y, int width, int height) => x >= 0 && y >= 0 && x < width && y < height;

        private void Shuffle((int dx, int dy)[] array)
        {
            var rand = new Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        public void PrintMaze(int[,] maze, System.Collections.Generic.List<(int x, int y)>? path = null)
        {
            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    if (path != null && path.Contains((x, y)))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write("██");
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write(maze[y, x] == 1 ? "  " : "██");
                    }
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }
    }
}
