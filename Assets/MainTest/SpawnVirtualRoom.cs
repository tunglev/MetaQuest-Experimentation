using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meta.XR.MRUtilityKit;
using Oculus.Platform;
using UnityEngine;

public class SpawnVirtualRoom : MonoBehaviour
{
    //[SerializeField] private GameObject prefab;
    private void Start() {
        var room = SpawnRoom();
        MRUK.Instance.LoadSceneFromPrefab(room, true);
        Destroy(room);
    }

    #region Start and End Points
    [SerializeField] private GameObject _pointVisualize;

    private GameObject _startPoint;
    private GameObject _endPoint;

    [ContextMenu("Spawn Start and End points")]
    void SpawnStartAndEndPoints()
    {
        if (_startPoint != null) Destroy(_startPoint);
        if (_endPoint != null) Destroy(_endPoint);

        Vector3 startPos = (Vector3)MRUK.Instance.GetCurrentRoom().GenerateRandomPositionInRoom(1, true);
        Vector3 endPos = (Vector3)MRUK.Instance.GetCurrentRoom().GenerateRandomPositionInRoom(1, true);

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
        var room = new GameObject("Custom Room").transform;
        var top = Instantiate(_data.wallPrefab, room);
        top.localPosition = new Vector3(_data.width / 2, 0, _data.length / 2);
        top.localEulerAngles = Vector3.zero;
        top.localScale = new Vector2(_data.width, _data.wallHeight);

        var bottom = Instantiate(_data.wallPrefab, room);
        bottom.localPosition = new Vector3(_data.width / 2, 0, -1 * _data.length / 2);
        bottom.localEulerAngles = new Vector3(0,180,0);
        bottom.localScale = new Vector2(_data.width, _data.wallHeight);

        var left = Instantiate(_data.wallPrefab, room);
        left.localPosition = Vector3.zero;
        left.localEulerAngles = new Vector3(0, -90, 0);
        left.localScale = new Vector2(_data.length, _data.wallHeight);

        var right = Instantiate(_data.wallPrefab, room);
        right.localPosition = new Vector3(_data.width, 0, 0);
        right.localEulerAngles = new Vector3(0, 90, 0);
        right.localScale = new Vector2(_data.length, _data.wallHeight);

        for (int i=0; i<room.childCount;i++) {
            room.GetChild(i).name = "WALL_FACE";
        }

        room.gameObject.SetActive(false);
        return room.gameObject;
    }
    #endregion

}
