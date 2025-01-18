using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainTestHandler : MonoBehaviour
{
    private SpawnVirtualRoom _virtualRoom;
    private BlindModeHandler _blindModeHandler;

    private void Awake()
    {
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
    }
    private void SpawnNewRoom() {
        FindObjectOfType<SphereGrow>().ResetSphere();
        _virtualRoom.SpawnNewRoomAsMRUKRoom();
        _blindModeHandler.ReapplyBlindMode();
    }

    #if UNITY_EDITOR
    private void OnGUI()
    {
        var myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 50;
        GUILayout.BeginArea(new Rect(10, 10, 300, 900));
        
            if (GUILayout.Button("SpawnNewRoom")) SpawnNewRoom();
            if (GUILayout.Button("Trigger")) FindObjectOfType<EncodingRunner>().TestTrigger();
            if (GUILayout.Button("Toggle BlindMode")) _blindModeHandler.ToogleBlindMode();

        GUILayout.EndArea();
    }
    #endif
}
