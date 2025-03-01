using System;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class SpawnVirtualRoom : MonoBehaviour
{
    private void OnValidate() {
        ValidateRoomGenerationParameters();
    }

    [ContextMenu("SpawnNewRoomAsMRUKRoom")]
    public void SpawnNewRoomAsMRUKRoom() {
#if UNITY_EDITOR

#else
            DemoPlayAreaDimensionsOnEditor();
#endif
        var room = _roomprefab == null ? SpawnTempRoom() : Instantiate(_roomprefab);
        MRUK.Instance.LoadSceneFromPrefab(room, true);
        Destroy(room);
        GenerateStartNode();
        GenerateGoalNode();
    }

    private void DemoPlayAreaDimensionsOnEditor() {
        var dimension = FindObjectOfType<PlayAreaAligner>().GetPlayAreaDimensions();
        if (!_useFixedRoomSize) {
            _data.roomSize.width = dimension.x;
            _data.roomSize.length = dimension.z;
        }
    }


    #region Generate Temp Room
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

        public int doorwayCount;
        public GenerationLimit doorWidthGen;
        internal DoorwayData[] doorwayData;
    }
    [SerializeField] private bool _useFixedRoomSize;
    [SerializeField] private GameObject _roomprefab;
    [SerializeField] private RoomGeneratorData _data;

    private void ValidateRoomGenerationParameters()
    {
        if (_data.doorWidthGen.max > _data.roomSize.width) throw new Exception("Door width cannot be greater than room width");
    }


    const float WALLHEIGHT = 2.5f;
    private GameObject SpawnTempRoom()
    {
        var room = new GameObject("Temp Room");
        GenerateFourWalls(room.transform);
        
        // int doorwaySpawned = 0;
        // while (doorwaySpawned < _data.doorwayCount) {
        //     GenerateDoorways(room.transform);
        //     doorwaySpawned++;
        // }
        (m_gridStartPos, m_gridGoalPos) = FindObjectOfType<MazeSpawner>().GenerateAndSpawnMaze(room.transform, _data.roomSize.width, _data.roomSize.length);
        
        room.transform.position += new Vector3(-0.5f * _data.roomSize.width,WALLHEIGHT * 0.5f, 0);
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

        for (int i = 0; i < room.transform.childCount; i++)
        {
            room.transform.GetChild(i).name = "WALL_FACE";
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

            rightWall.name = "STORAGE";
            leftWall.name = "STORAGE";
        }

    }


    #endregion
    #region Generate StartNode
    [Header("Start Node")]
    [SerializeField] private GameObject m_startObjectPrefab;
    private Vector3 m_gridStartPos;
    private GameObject m_currentStartGameobject;

    private void GenerateStartNode() {
        if (m_currentStartGameobject != null) Destroy(m_currentStartGameobject.gameObject);
        m_currentStartGameobject = Instantiate(m_startObjectPrefab, m_gridStartPos, Quaternion.identity);
    }
    #endregion
    #region Generate GoalNode
    public Goal CurrentGoal {get; private set;}
    [Header("Goal Node")]
    [SerializeField] private Goal _goalNodePrefab;
    [SerializeField] GoalPositionType m_goalPosType;
    [SerializeField] GenerationLimit m_goalHeightRange;
    private Vector3 m_gridGoalPos;

    public enum GoalPositionType {
        Fixed = 0, GridRandom = 1, PositionRandom = 2
    }

    private void GenerateGoalNode()
    {
        if (CurrentGoal != null) Destroy(CurrentGoal.gameObject);
        Vector3? goalPos = null;
        switch (m_goalPosType) {
            case GoalPositionType.Fixed:
                break;
            case GoalPositionType.GridRandom:
                m_gridGoalPos.y = m_goalHeightRange.GetRandomVal();
                goalPos = m_gridGoalPos;
                break;
            case GoalPositionType.PositionRandom:
                goalPos = MRUK.Instance.GetCurrentRoom().GenerateRandomPositionInRoom(minDistanceToSurface: 0.4f, avoidVolumes: true);
                if (goalPos == null)
                { // regenerate room if impossible to get pos in room
                    SpawnTempRoom();
                    return;
                }
                break;
            default:
                throw new Exception("Goal position type not defined");
        }
        
        CurrentGoal = Instantiate(_goalNodePrefab, (Vector3) goalPos, Quaternion.identity);
        CurrentGoal.name = "GOAL";
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
