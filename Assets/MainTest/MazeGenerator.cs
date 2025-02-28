using System;
using System.Collections.Generic;

public class MazeGenerator
{
    private static readonly Random _random = new Random();

    public (bool[,] HorizontalWalls, bool[,] VerticalWalls) Generate(int width, int length, int maxWalls = -1)
    {
        if (width <= 0 || length <= 0)
            throw new ArgumentException("Width and length must be positive integers.");

        // Initialize wall grids (true = wall present)
        bool[,] horizontalWalls = new bool[length + 1, width];
        bool[,] verticalWalls = new bool[length, width + 1];
        bool[,] visited = new bool[length, width];

        // First, create a fully connected maze (minimum spanning tree)
        // Initialize with all walls
        for (int i = 0; i <= length; i++)
            for (int j = 0; j < width; j++)
                horizontalWalls[i, j] = true;

        for (int i = 0; i < length; i++)
            for (int j = 0; j <= width; j++)
                verticalWalls[i, j] = true;

        // Generate a fully connected maze using depth-first search
        Stack<(int row, int col)> stack = new Stack<(int row, int col)>();
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

        // At this point, we have a minimal maze with the minimum number of walls
        // for a fully connected maze (each cell is reachable)

        // If maxWalls is -1, return the fully connected maze
        if (maxWalls == -1)
        {
            RemoveOuterBoundaries(horizontalWalls, verticalWalls, width, length);
            return (horizontalWalls, verticalWalls);
        }

        // Create a list of all remaining internal walls
        List<(int row, int col, bool isHorizontal)> remainingWalls = new List<(int row, int col, bool isHorizontal)>();

        // Add all horizontal walls (excluding outer boundaries)
        for (int i = 1; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if (horizontalWalls[i, j])
                {
                    remainingWalls.Add((i, j, true));
                }
            }
        }

        // Add all vertical walls (excluding outer boundaries)
        for (int i = 0; i < length; i++)
        {
            for (int j = 1; j < width; j++)
            {
                if (verticalWalls[i, j])
                {
                    remainingWalls.Add((i, j, false));
                }
            }
        }

        // Shuffle the list of walls
        ShuffleList(remainingWalls);

        // Reset all internal walls to false (remove them)
        for (int i = 1; i < length; i++)
            for (int j = 0; j < width; j++)
                horizontalWalls[i, j] = false;

        for (int i = 0; i < length; i++)
            for (int j = 1; j < width; j++)
                verticalWalls[i, j] = false;

        // Add back exactly maxWalls walls
        int wallsToAdd = Math.Min(maxWalls, remainingWalls.Count);
        for (int i = 0; i < wallsToAdd; i++)
        {
            var (row, col, isHorizontal) = remainingWalls[i];
            if (isHorizontal)
            {
                horizontalWalls[row, col] = true;
            }
            else
            {
                verticalWalls[row, col] = true;
            }
        }

        // Remove outer boundary walls
        RemoveOuterBoundaries(horizontalWalls, verticalWalls, width, length);

        return (horizontalWalls, verticalWalls);
    }

    public ((int row, int col) start, (int row, int col) end) GetStartAndEndCells(int width, int length)
    {
        // Decide whether to use horizontal or vertical opposing sides
        bool useHorizontalOpposition = width >= length;

        if (useHorizontalOpposition)
        {
            // Start from left side (first column), end at right side (last column)
            int startRow = _random.Next(0, length);
            int endRow = _random.Next(0, length);

            return ((startRow, 0), (endRow, width - 1));
        }
        else
        {
            // Start from top side (first row), end at bottom side (last row)
            int startCol = _random.Next(0, width);
            int endCol = _random.Next(0, width);

            return ((0, startCol), (length - 1, endCol));
        }
    }

    // Overload that automatically selects based on the dimensions of the maze
    public ((int row, int col) start, (int row, int col) end) GetStartAndEndCells(bool[,] horizontalWalls, bool[,] verticalWalls)
    {
        int length = horizontalWalls.GetLength(0) - 1;
        int width = horizontalWalls.GetLength(1);

        return GetStartAndEndCells(width, length);
    }

    // Alternative implementation that lets you specify which sides to use
    public ((int row, int col) start, (int row, int col) end) GetStartAndEndCells(int width, int length, string startSide, string endSide)
    {
        if (startSide == endSide)
            throw new ArgumentException("Start and end sides must be different");

        Dictionary<string, Func<(int row, int col)>> cellSelectors = new Dictionary<string, Func<(int row, int col)>>
        {
            ["top"] = () => (0, _random.Next(0, width)),
            ["bottom"] = () => (length - 1, _random.Next(0, width)),
            ["left"] = () => (_random.Next(0, length), 0),
            ["right"] = () => (_random.Next(0, length), width - 1)
        };

        if (!cellSelectors.ContainsKey(startSide.ToLower()) || !cellSelectors.ContainsKey(endSide.ToLower()))
            throw new ArgumentException("Sides must be one of: top, bottom, left, right");

        return (cellSelectors[startSide.ToLower()](), cellSelectors[endSide.ToLower()]());
    }

    private void RemoveOuterBoundaries(bool[,] horizontalWalls, bool[,] verticalWalls, int width, int length)
    {
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

    private void ShuffleList<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
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

    // Backward compatibility for original method
    public (bool[,] HorizontalWalls, bool[,] VerticalWalls) Generate(int n)
    {
        return Generate(n, n);
    }

    // Backward compatibility for width and length without maxWalls
    public (bool[,] HorizontalWalls, bool[,] VerticalWalls) Generate(int width, int length)
    {
        return Generate(width, length, -1);
    }
}