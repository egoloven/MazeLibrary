using System;
using System.Collections.Generic;

namespace MazeLibrary
{
    public class Pathfinder
    {
        public List<(int, int)> FindPathDFS(int[,] maze, (int x, int y) start, (int x, int y) end)
        {
            var path = new List<(int, int)>();
            var visited = new HashSet<(int, int)>();
            if (DFS(maze, start, end, path, visited))
                return path;
            return new List<(int, int)>();
        }

        private bool DFS(int[,] maze, (int x, int y) current, (int x, int y) end, List<(int, int)> path, HashSet<(int, int)> visited)
        {
            if (current == end) 
            {
                path.Add(current);
                return true;
            }

            if (!IsInBounds(current, maze) || maze[current.Item2, current.Item1] == 0 || visited.Contains(current))
                return false;

            visited.Add(current);
            path.Add(current);

            var directions = new (int dx, int dy)[] { (0, -1), (1, 0), (0, 1), (-1, 0) };
            foreach (var (dx, dy) in directions)
            {
                if (DFS(maze, (current.Item1 + dx, current.Item2 + dy), end, path, visited))
                    return true;
            }

            path.RemoveAt(path.Count - 1);
            return false;
        }

        private bool IsInBounds((int x, int y) point, int[,] maze) =>
            point.Item1 >= 0 && point.Item2 >= 0 && point.Item1 < maze.GetLength(1) && point.Item2 < maze.GetLength(0);

        public List<(int, int)> FindPathAStar(int[,] maze, (int x, int y) start, (int x, int y) end)
        {
            var openSet = new PriorityQueue<(int x, int y), double>();
            var cameFrom = new Dictionary<(int x, int y), (int x, int y)>();
            var gScore = new Dictionary<(int x, int y), double>();
            var fScore = new Dictionary<(int x, int y), double>();

            gScore[start] = 0;
            fScore[start] = Heuristic(start, end);
            openSet.Enqueue(start, fScore[start]);

            var inOpenSet = new HashSet<(int x, int y)> { start };  // Tracking elements in the open set

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                inOpenSet.Remove(current);

                if (current == end)
                    return ReconstructPath(cameFrom, current);

                foreach (var (dx, dy) in new (int dx, int dy)[] { (0, -1), (1, 0), (0, 1), (-1, 0) })
                {
                    var neighbor = (current.Item1 + dx, current.Item2 + dy);
                    if (!IsInBounds(neighbor, maze) || maze[neighbor.Item2, neighbor.Item1] == 0) 
                        continue;

                    var tentativeGScore = gScore.GetValueOrDefault(current, double.MaxValue) + 1;

                    if (tentativeGScore < gScore.GetValueOrDefault(neighbor, double.MaxValue))
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, end);

                        if (!inOpenSet.Contains(neighbor))
                        {
                            openSet.Enqueue(neighbor, fScore[neighbor]);
                            inOpenSet.Add(neighbor);
                        }
                    }
                }
            }

            return new List<(int, int)>();
        }

        public List<(int, int)> FindPathDijkstra(int[,] maze, (int x, int y) start, (int x, int y) end)
        {
            var distances = new Dictionary<(int x, int y), double>();
            var cameFrom = new Dictionary<(int x, int y), (int x, int y)>();
            var pq = new PriorityQueue<(int x, int y), double>();
            var inPriorityQueue = new HashSet<(int x, int y)>();  // Tracking elements in the priority queue

            distances[start] = 0;
            pq.Enqueue(start, 0);
            inPriorityQueue.Add(start);

            while (pq.Count > 0)
            {
                var current = pq.Dequeue();
                inPriorityQueue.Remove(current);

                if (current == end)
                    return ReconstructPath(cameFrom, current);

                foreach (var (dx, dy) in new (int dx, int dy)[] { (0, -1), (1, 0), (0, 1), (-1, 0) })
                {
                    var neighbor = (current.Item1 + dx, current.Item2 + dy);
                    if (!IsInBounds(neighbor, maze) || maze[neighbor.Item2, neighbor.Item1] == 0) 
                        continue;

                    var newDist = distances.GetValueOrDefault(current, double.MaxValue) + 1;

                    if (newDist < distances.GetValueOrDefault(neighbor, double.MaxValue))
                    {
                        distances[neighbor] = newDist;
                        cameFrom[neighbor] = current;
                        if (!inPriorityQueue.Contains(neighbor))
                        {
                            pq.Enqueue(neighbor, newDist);
                            inPriorityQueue.Add(neighbor);
                        }
                    }
                }
            }

            return new List<(int, int)>();
        }

        private List<(int, int)> ReconstructPath(Dictionary<(int x, int y), (int x, int y)> cameFrom, (int x, int y) current)
        {
            var path = new List<(int, int)>();
            while (cameFrom.ContainsKey(current))
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Add(current);
            path.Reverse();
            return path;
        }

        private double Heuristic((int x, int y) a, (int x, int y) b) =>
            Math.Abs(a.Item1 - b.Item1) + Math.Abs(a.Item2 - b.Item2);
    }
}
