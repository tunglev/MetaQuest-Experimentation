using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainTestHandler : MonoBehaviour
{
    public static MainTestHandler Instance {get;private set;}

    public Action OnNewRoomSpanwed;
    public Action<bool> OnBlindModeToggled;
    public Action OnGoalReached;

    private SpawnVirtualRoom _virtualRoom;
    private BlindModeHandler _blindModeHandler;

    private void Awake()
    {
        if (Instance==null)Instance = this;
        else Destroy(this);
        _virtualRoom = FindObjectOfType<SpawnVirtualRoom>();
        _blindModeHandler = FindObjectOfType<BlindModeHandler>();
        FindObjectOfType<OVRCameraRig>().rightControllerAnchor.gameObject.AddComponent<ReachGoalHandler>();
    }

    private void Start() {
        SpawnNewRoom();
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            SpawnNewRoom();
        }
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            _blindModeHandler.ToogleBlindMode();
        }
    }
    private void SpawnNewRoom() {
        _virtualRoom.SpawnNewRoomAsMRUKRoom();
        _blindModeHandler.ReapplyBlindMode();
        OnNewRoomSpanwed?.Invoke();
    }

    #if UNITY_EDITOR
    private void OnGUI()
    {
        var myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 50;
        GUILayout.BeginArea(new Rect(10, 10, 300, 900));
        
            if (GUILayout.Button("SpawnNewRoom")) SpawnNewRoom();
            if (GUILayout.Button("TriggerDown")) FindObjectOfType<EncodingRunner>().TriggerDown();
            if (GUILayout.Button("TriggerUp")) FindObjectOfType<EncodingRunner>().TriggerUp();
        if (GUILayout.Button("Toggle BlindMode")) {
                _blindModeHandler.ToogleBlindMode();
            }

        GUILayout.EndArea();
    }
    #endif
}
