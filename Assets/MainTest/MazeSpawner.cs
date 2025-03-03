using UnityEngine;
using UnityEngine.UIElements;

public class MazeSpawner : MonoBehaviour
{
    [Header("Maze Settings")]
    public Vector2Int mazeGridSize = new (4,5);
    public int maxNumberOfWalls = 10;
    private float roomLength;
    private float roomWidth;

    [Header("Wall Settings")]
    public GameObject wallPrefab;
    public float wallHeight = 2.5f;
    public float wallThickness = 0.15f;
    public string wallName = "STORAGE";

    private MazeGenerator mazeGenerator;
    private float cellWidth;
    private float cellLength;

    private Transform parentObj;


    [ContextMenu("GenerateAndSpawnMaze")]
    public (Vector3 startPos, Vector3 endPos) GenerateAndSpawnMaze(Transform roomGameobject, float roomWidth, float roomLength)
    {
        //divide more on a larger side
        int largerVal = Mathf.Max(mazeGridSize.x, mazeGridSize.y);
        int smallerVal = Mathf.Min(mazeGridSize.x, mazeGridSize.y);
        mazeGridSize.x = roomWidth > roomLength ? largerVal : smallerVal;
        mazeGridSize.y = roomLength > roomWidth ? largerVal : smallerVal;

        parentObj = roomGameobject;
        this.roomLength = roomLength;
        this.roomWidth =roomWidth;
        mazeGenerator = new MazeGenerator();
        var (horizontalWalls, verticalWalls) = mazeGenerator.Generate(mazeGridSize.x, mazeGridSize.y, maxNumberOfWalls);

        cellWidth = roomWidth / mazeGridSize.x;
        cellLength = roomLength / mazeGridSize.y;

        SpawnCombinedWalls(horizontalWalls, verticalWalls);

        // start and end pos
        var (startCell, endCell) = mazeGenerator.GetStartAndEndCells(mazeGridSize.x, mazeGridSize.y);
        return (GridPosToWorldPos(startCell), GridPosToWorldPos(endCell));
    }

    public Vector3 GridPosToWorldPos((int row, int col) cell) {
        return GridPosToWorldPos(cell, new(0.5f, 0.5f));
    }

    public Vector3 GridPosToWorldPos((int row, int col) cell, Vector2 normalizedPositionOnGrid) {
        // normalizedPositionOnGrid = 0.5, 0.5: center of grid
        return parentObj.TransformPoint(new Vector3((cell.col + normalizedPositionOnGrid.x) * cellWidth, 0, (cell.row + normalizedPositionOnGrid.y) * cellLength) + new Vector3(-roomWidth * 0.5f, 0, -roomLength * 0.5f));
    }

    private void SpawnCombinedWalls(bool[,] horizontalWalls, bool[,] verticalWalls)
    {
        // Process horizontal walls (between rows)
        for (int row = 0; row < horizontalWalls.GetLength(0); row++)
        {
            for (int col = 0; col < horizontalWalls.GetLength(1);)
            {
                if (horizontalWalls[row, col])
                {
                    int runLength = FindHorizontalRun(horizontalWalls, row, col);
                    CreateHorizontalWall(row, col, runLength);
                    col += runLength;
                }
                else
                {
                    col++;
                }
            }
        }

        // Process vertical walls (between columns)
        for (int col = 0; col < verticalWalls.GetLength(1); col++)
        {
            for (int row = 0; row < verticalWalls.GetLength(0);)
            {
                if (verticalWalls[row, col])
                {
                    int runLength = FindVerticalRun(verticalWalls, row, col);
                    CreateVerticalWall(col, row, runLength);
                    row += runLength;
                }
                else
                {
                    row++;
                }
            }
        }
    }

    private int FindHorizontalRun(bool[,] walls, int row, int startCol)
    {
        int runLength = 1;
        while (startCol + runLength < walls.GetLength(1) &&
               walls[row, startCol + runLength])
        {
            runLength++;
        }
        return runLength;
    }

    private int FindVerticalRun(bool[,] walls, int startRow, int col)
    {
        int runLength = 1;
        while (startRow + runLength < walls.GetLength(0) &&
               walls[startRow + runLength, col])
        {
            runLength++;
        }
        return runLength;
    }

    private void CreateHorizontalWall(int row, int startCol, int runLength)
    {
        float totalWidth = runLength * cellWidth;
        float centerX = (startCol + runLength * 0.5f) * cellWidth;
        float zPosition = row * cellLength;

        Vector3 position = new Vector3(
            centerX,
            wallHeight / 2f,
            zPosition
        );

        Vector3 scale = new Vector3(
            totalWidth + wallThickness,
            wallHeight,
            wallThickness
        );

        CreateWall(position, scale);
    }

    private void CreateVerticalWall(int col, int startRow, int runLength)
    {
        float totalLength = runLength * cellLength;
        float centerZ = (startRow + runLength * 0.5f) * cellLength;
        float xPosition = col * cellWidth;

        Vector3 position = new Vector3(
            xPosition,
            wallHeight / 2f,
            centerZ
        );

        Vector3 scale = new Vector3(
            wallThickness,
            wallHeight,
            totalLength + wallThickness
        );

        CreateWall(position, scale);
    }

    private void CreateWall(Vector3 position, Vector3 scale)
    {
        GameObject wall = Instantiate(wallPrefab, parentObj);
        wall.transform.localPosition = position + new Vector3(0, -wallHeight *0.5f,  -roomLength * 0.5f);
        wall.transform.localScale = scale;
        wall.name = wallName;
    }
}