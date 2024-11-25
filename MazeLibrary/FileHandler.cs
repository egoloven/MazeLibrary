using System;
using System.IO;

namespace MazeLibrary
{
    public class FileHandler
    {
        public void SaveMazeToFile(int[,] maze, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                for (int y = 0; y < maze.GetLength(0); y++)
                {
                    for (int x = 0; x < maze.GetLength(1); x++)
                    {
                        writer.Write(maze[y, x]);
                    }
                    writer.WriteLine();
                }
            }
        }

        public int[,] LoadMazeFromFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            int height = lines.Length;
            int width = lines[0].Length;
            var maze = new int[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    maze[y, x] = lines[y][x] - '0';
                }
            }

            return maze;
        }
    }
}
