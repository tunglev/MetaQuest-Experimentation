using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meta.XR.MRUtilityKit;
using Oculus.Platform;
using UnityEngine;

public class SpawnVirtualRoom : MonoBehaviour
{
    #region Start and End Points
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
    #endregion


    #region Generate Room
    [Serializable]
    public struct RoomGeneratorData
    {
        public float width; // x axis
        public float length; // z axis
        public float wallHeight;
        public Transform wallPrefab;
    }
    [SerializeField] private RoomGeneratorData _data;

    [ContextMenu("Spawn Room")]
    private GameObject SpawnRoom()
    {
        var parent = new GameObject("Custom Room").transform;
        var top = Instantiate(_data.wallPrefab, parent);
        top.localPosition = new Vector3(_data.width / 2, 0, _data.length / 2);
        top.localEulerAngles = Vector3.zero;
        top.localScale = new Vector2(_data.width, _data.wallHeight);

        var bottom = Instantiate(_data.wallPrefab, parent);
        bottom.localPosition = new Vector3(_data.width / 2, 0, -1 * _data.length / 2);
        bottom.localEulerAngles = new Vector3(0,180,0);
        bottom.localScale = new Vector2(_data.width, _data.wallHeight);

        var left = Instantiate(_data.wallPrefab, parent);
        left.localPosition = Vector3.zero;
        left.localEulerAngles = new Vector3(0, -90, 0);
        left.localScale = new Vector2(_data.length, _data.wallHeight);

        var right = Instantiate(_data.wallPrefab, parent);
        right.localPosition = new Vector3(_data.width, 0, 0);
        right.localEulerAngles = new Vector3(0, 90, 0);
        right.localScale = new Vector2(_data.length, _data.wallHeight);

        for (int i=0; i<parent.childCount;i++) {
            parent.GetChild(i).name = "WALL_FACE";
        }
        return parent.gameObject;
    }
    #endregion

}
