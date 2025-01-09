using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Meta.XR.MRUtilityKit;
using Oculus.Platform;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnVirtualRoom : MonoBehaviour
{
    private void OnValidate() {
        ValidateRoomGenerationParameters();
    }

  
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
    public struct DoorwayData {
        public float width;
        public float positionLeftOffset; // Distance from left edge of wall
    }
    [Serializable]
    public struct RoomSizeData {
        public float width; // x axis
        public float length; // z axis
    }
    [Serializable]
    public struct RoomGeneratorData
    {
        public RoomSizeData roomSize;
        public float wallHeight;
        public Transform wallPrefab;

        public GenerationLimit doorWidthGen;
        internal DoorwayData[] doorwayData;
    }
    [SerializeField] private RoomGeneratorData _data;

    private void ValidateRoomGenerationParameters()
    {
        if (_data.doorWidthGen.max > _data.roomSize.width) throw new Exception("Door width cannot be greater than room width");
    }


    [ContextMenu("Spawn Room")]
    private GameObject SpawnRoom()
    {
        var room = new GameObject("Custom Room");
        GenerateFourWalls(room.transform);
        GenerateDoorways(room.transform);

        room.SetActive(false);
        return room;
    }


    private void GenerateFourWalls(Transform room) {
        var top = Instantiate(_data.wallPrefab, room);
        var width = _data.roomSize.width;
        var length = _data.roomSize.length;
        top.localPosition = new Vector3(width / 2, 0, length / 2);
        top.localEulerAngles = Vector3.zero;
        top.localScale = new Vector3(width, _data.wallHeight,1);

        var bottom = Instantiate(_data.wallPrefab, room);
        bottom.localPosition = new Vector3(width / 2, 0, -1 * length / 2);
        bottom.localEulerAngles = new Vector3(0, 180, 0);
        bottom.localScale = new Vector3(width, _data.wallHeight,1);

        var left = Instantiate(_data.wallPrefab, room);
        left.localPosition = Vector3.zero;
        left.localEulerAngles = new Vector3(0, -90, 0);
        left.localScale = new Vector3(length, _data.wallHeight,1);

        var right = Instantiate(_data.wallPrefab, room);
        right.localPosition = new Vector3(width, 0, 0);
        right.localEulerAngles = new Vector3(0, 90, 0);
        right.localScale = new Vector3(length, _data.wallHeight, 1);

        for (int i = 0; i < room.childCount; i++)
        {
            room.GetChild(i).name = "WALL_FACE";
        }
    }

    private void GenerateDoorways(Transform room)
    {
        var Zpos = Random.Range(-_data.roomSize.length / 2, _data.roomSize.length / 2);
        var doorWidth = _data.doorWidthGen.GetRandomVal();
        var leftOffset = Random.Range(0f, _data.roomSize.width - doorWidth);
        var leftWallSize = leftOffset;
        var leftWallPos = new Vector3(leftWallSize / 2, 0, Zpos);

        var leftWall = Instantiate(_data.wallPrefab, room);
        leftWall.localPosition = leftWallPos;
        leftWall.localScale = new(leftWallSize, _data.wallHeight , 1);

        var rightWallSize = _data.roomSize.width - leftWallSize - doorWidth;
        var rightWallPos = new Vector3(leftWallSize + doorWidth + rightWallSize / 2, 0, Zpos);
        if (rightWallPos.x < _data.roomSize.width) {
            var rightWall = Instantiate(_data.wallPrefab, room);
            rightWall.localPosition = rightWallPos;
            rightWall.localScale = new(rightWallSize, _data.wallHeight, 1);
        }

    }
    #endregion

}

[Serializable]
public class GenerationLimit
{
    [SerializeField] internal float min;
    [SerializeField] internal float max;

    public float GetRandomVal() {
        return Random.Range(min, max);
    }
}
