using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class SpawnVirtualRoom : MonoBehaviour
{
    [SerializeField] private GameObject _pointVisualize;

    private GameObject _startPoint;
    private GameObject _endPoint;

    [ContextMenu("Spawn Start and End points")]
    void SpawnStartAndEndPoints()
    {
        var floor = MRUK.Instance.GetCurrentRoom().FloorAnchor;
        List<Vector2> boundaryPoints = floor.PlaneBoundary2D; // local position
        PointGenerator generator = new PointGenerator();
        Vector2[] pointsInside = generator.GeneratePointsInsidePolygon(boundaryPoints.ToArray(), 2);
        if (_startPoint != null) Destroy(_startPoint);
        if (_endPoint != null) Destroy(_endPoint);

        Vector3 startPos =  new Vector3(pointsInside[0].x, 0, pointsInside[0].y);
        startPos = floor.transform.TransformPoint(startPos);
        Vector3 endPos = new Vector3(pointsInside[1].x, 0, pointsInside[1].y);
        endPos = floor.transform.TransformPoint(endPos);
        
        _startPoint = Instantiate(_pointVisualize, startPos, Quaternion.identity);
        _endPoint = Instantiate(_pointVisualize, endPos, Quaternion.identity);
    }

}
