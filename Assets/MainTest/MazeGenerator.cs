using System;
using System.Collections.Generic;

public class MazeGenerator
{
    private static readonly Random _random = new Random();

    public (bool[,] HorizontalWalls, bool[,] VerticalWalls) Generate(int width, int length)
    {
        if (width <= 0 || length <= 0)
            throw new ArgumentException("Width and length must be positive integers.");

        // Initialize wall grids (true = wall present)
        bool[,] horizontalWalls = new bool[length + 1, width];
        bool[,] verticalWalls = new bool[length, width + 1];
        bool[,] visited = new bool[length, width];
        Stack<(int row, int col)> stack = new Stack<(int row, int col)>();

        // Initialize all walls (including outer edges)
        InitializeWalls(horizontalWalls, verticalWalls, width, length);

        // Start from the top-left corner (0,0)
        visited[0, 0] = true;
        stack.Push((0, 0));

        while (stack.Count > 0)
        {
            var (row, col) = stack.Pop();
            var neighbors = GetUnvisitedNeighbors(row, col, width, length, visited);

            if (neighbors.Count > 0)
            {
                stack.Push((row, col));
                var ((nRow, nCol), direction) = neighbors[_random.Next(neighbors.Count)];

                // Remove the wall between current cell and neighbor
                RemoveWall(row, col, nRow, nCol, direction, horizontalWalls, verticalWalls);
                visited[nRow, nCol] = true;
                stack.Push((nRow, nCol));
            }
        }

        return (horizontalWalls, verticalWalls);
    }

    private void InitializeWalls(bool[,] horizontalWalls, bool[,] verticalWalls, int width, int length)
    {
        // Set all walls initially to true
        for (int i = 0; i <= length; i++)
            for (int j = 0; j < width; j++)
                horizontalWalls[i, j] = true;

        for (int i = 0; i < length; i++)
            for (int j = 0; j <= width; j++)
                verticalWalls[i, j] = true;

        // Remove outer boundaries
        for (int j = 0; j < width; j++)
        {
            horizontalWalls[0, j] = false;    // Top edge
            horizontalWalls[length, j] = false;    // Bottom edge
        }

        for (int i = 0; i < length; i++)
        {
            verticalWalls[i, 0] = false;      // Left edge
            verticalWalls[i, width] = false;      // Right edge
        }
    }

    private void RemoveWall(int row, int col, int nRow, int nCol, string direction,
                          bool[,] horizontalWalls, bool[,] verticalWalls)
    {
        switch (direction)
        {
            case "North":
                horizontalWalls[row, col] = false;    // Remove north wall of current cell
                break;
            case "South":
                horizontalWalls[row + 1, col] = false; // Remove south wall of current cell
                break;
            case "East":
                verticalWalls[row, col + 1] = false;  // Remove east wall of current cell
                break;
            case "West":
                verticalWalls[row, col] = false;      // Remove west wall of current cell
                break;
        }
    }

    private List<((int nRow, int nCol), string direction)> GetUnvisitedNeighbors(int row, int col, int width, int length, bool[,] visited)
    {
        var neighbors = new List<((int, int), string)>();

        // North neighbor
        if (row > 0 && !visited[row - 1, col]) neighbors.Add(((row - 1, col), "North"));
        // South neighbor
        if (row < length - 1 && !visited[row + 1, col]) neighbors.Add(((row + 1, col), "South"));
        // East neighbor
        if (col < width - 1 && !visited[row, col + 1]) neighbors.Add(((row, col + 1), "East"));
        // West neighbor
        if (col > 0 && !visited[row, col - 1]) neighbors.Add(((row, col - 1), "West"));

        return neighbors;
    }

    // Keep the original method for backward compatibility
    public (bool[,] HorizontalWalls, bool[,] VerticalWalls) Generate(int n)
    {
        return Generate(n, n);
    }
}