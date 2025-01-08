using UnityEngine;

public class PointGenerator
{
    // Point in polygon check using ray casting
    private bool IsPointInPolygon(Vector2 point, Vector2[] vertices)
    {
        bool inside = false;
        int j = vertices.Length - 1;

        for (int i = 0; i < vertices.Length; i++)
        {
            if (((vertices[i].y > point.y) != (vertices[j].y > point.y)) &&
                (point.x < (vertices[j].x - vertices[i].x) * (point.y - vertices[i].y)
                / (vertices[j].y - vertices[i].y) + vertices[i].x))
            {
                inside = !inside;
            }
            j = i;
        }

        return inside;
    }

    public Vector2[] GeneratePointsInsidePolygon(Vector2[] boundaryPoints, int numberOfPoints)
    {
        // Find min and max coordinates to establish bounding box
        float minX = float.MaxValue, maxX = float.MinValue;
        float minY = float.MaxValue, maxY = float.MinValue;

        foreach (Vector2 point in boundaryPoints)
        {
            minX = Mathf.Min(minX, point.x);
            maxX = Mathf.Max(maxX, point.x);
            minY = Mathf.Min(minY, point.y);
            maxY = Mathf.Max(maxY, point.y);
        }

        Vector2[] result = new Vector2[numberOfPoints];
        int foundPoints = 0;

        while (foundPoints < numberOfPoints)
        {
            // Generate random point within bounding box
            Vector2 randomPoint = new Vector2(
                Random.Range(minX, maxX),
                Random.Range(minY, maxY)
            );

            // Check if point is inside polygon
            if (IsPointInPolygon(randomPoint, boundaryPoints))
            {
                result[foundPoints] = randomPoint;
                foundPoints++;
            }
        }

        return result;
    }
}

