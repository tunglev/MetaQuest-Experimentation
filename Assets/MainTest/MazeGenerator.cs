using System;
using System.Collections.Generic;

public class MazeGenerator
{
    private static readonly Random _random = new Random();

    public (bool[,] HorizontalWalls, bool[,] VerticalWalls) Generate(int n)
    {
        if (n <= 0)
            throw new ArgumentException("Size n must be a positive integer.", nameof(n));

        // Initialize wall grids (true = wall present)
        bool[,] horizontalWalls = new bool[n + 1, n];
        bool[,] verticalWalls = new bool[n, n + 1];
        bool[,] visited = new bool[n, n];
        Stack<(int x, int y)> stack = new Stack<(int x, int y)>();

        // Initialize all walls (including outer edges)
        InitializeWalls(horizontalWalls, verticalWalls, n);

        // Start from the top-left corner (0,0)
        visited[0, 0] = true;
        stack.Push((0, 0));

        while (stack.Count > 0)
        {
            var (x, y) = stack.Pop();
            var neighbors = GetUnvisitedNeighbors(x, y, n, visited);

            if (neighbors.Count > 0)
            {
                stack.Push((x, y));
                var ((nx, ny), direction) = neighbors[_random.Next(neighbors.Count)];

                // Remove the wall between current cell and neighbor
                RemoveWall(x, y, nx, ny, direction, horizontalWalls, verticalWalls);
                visited[nx, ny] = true;
                stack.Push((nx, ny));
            }
        }

        return (horizontalWalls, verticalWalls);
    }

    private void InitializeWalls(bool[,] horizontalWalls, bool[,] verticalWalls, int n)
    {
        // Set all walls initially to true
        for (int i = 0; i <= n; i++)
            for (int j = 0; j < n; j++)
                horizontalWalls[i, j] = true;

        for (int i = 0; i < n; i++)
            for (int j = 0; j <= n; j++)
                verticalWalls[i, j] = true;

        // Remove outer boundaries
        for (int j = 0; j < n; j++)
        {
            horizontalWalls[0, j] = false;    // Top edge
            horizontalWalls[n, j] = false;    // Bottom edge
        }

        for (int i = 0; i < n; i++)
        {
            verticalWalls[i, 0] = false;      // Left edge
            verticalWalls[i, n] = false;      // Right edge
        }
    }

    private void RemoveWall(int x, int y, int nx, int ny, string direction,
                          bool[,] horizontalWalls, bool[,] verticalWalls)
    {
        switch (direction)
        {
            case "North":
                horizontalWalls[x, y] = false;    // Remove north wall of current cell
                break;
            case "South":
                horizontalWalls[x + 1, y] = false; // Remove south wall of current cell
                break;
            case "East":
                verticalWalls[x, y + 1] = false;  // Remove east wall of current cell
                break;
            case "West":
                verticalWalls[x, y] = false;      // Remove west wall of current cell
                break;
        }
    }

    private List<((int nx, int ny), string direction)> GetUnvisitedNeighbors(int x, int y, int n, bool[,] visited)
    {
        var neighbors = new List<((int, int), string)>();

        // North neighbor
        if (x > 0 && !visited[x - 1, y]) neighbors.Add(((x - 1, y), "North"));
        // South neighbor
        if (x < n - 1 && !visited[x + 1, y]) neighbors.Add(((x + 1, y), "South"));
        // East neighbor
        if (y < n - 1 && !visited[x, y + 1]) neighbors.Add(((x, y + 1), "East"));
        // West neighbor
        if (y > 0 && !visited[x, y - 1]) neighbors.Add(((x, y - 1), "West"));

        return neighbors;
    }
}