using UnityEngine;

public class MazeSpawner : MonoBehaviour
{
    [Header("Maze Settings")]
    public int mazeSize = 5;
    public float roomWidth = 30f;
    public float roomLength = 30f;

    [Header("Wall Settings")]
    public GameObject wallPrefab;
    public float wallHeight = 3f;
    public float wallThickness = 0.3f;
    public Material wallMaterial;

    private MazeGenerator mazeGenerator;
    private float cellWidth;
    private float cellLength;

    void Start()
    {
        GenerateAndSpawnMaze();
    }

    [ContextMenu("GenerateAndSpawnMaze")]
    public void GenerateAndSpawnMaze()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        mazeGenerator = new MazeGenerator();
        var (horizontalWalls, verticalWalls) = mazeGenerator.Generate(mazeSize);

        cellWidth = roomWidth / mazeSize;
        cellLength = roomLength / mazeSize;

        SpawnCombinedWalls(horizontalWalls, verticalWalls);
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
            totalWidth,
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
            totalLength
        );

        CreateWall(position, scale);
    }

    private void CreateWall(Vector3 position, Vector3 scale)
    {
        GameObject wall = Instantiate(wallPrefab, transform);
        wall.transform.localPosition = position;
        wall.transform.localScale = scale;

        if (wallMaterial != null)
            wall.GetComponent<Renderer>().material = wallMaterial;
    }
}