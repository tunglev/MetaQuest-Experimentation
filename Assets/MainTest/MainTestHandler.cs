using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MainTestHandler : MonoBehaviour
{
    public static MainTestHandler Instance {get;private set;}

    public Action OnNewRoomSpanwed;
    public Action<bool> OnBlindModeToggled;
    public Action OnGoalReached;
    public Action<int> OnEncodingChanged;
    public bool IsBlind {
        get {
            return _blindModeHandler.IsBlind;
        }
    }

    [Header("Controller Panel")]
    [SerializeField] GameObject _controllerPanel;
    [SerializeField] Button _spawnRoomBtn;
    [SerializeField] Button _blindModeBtn;
    [SerializeField] Toggle _fixRoomSizeToogle;
    [SerializeField] TMP_Dropdown _encodingPicker;

    private SpawnVirtualRoom _virtualRoom;
    private BlindModeHandler _blindModeHandler;

    private void Awake()
    {
        if (Instance==null)Instance = this;
        else Destroy(this);
        _virtualRoom = FindObjectOfType<SpawnVirtualRoom>();
        _blindModeHandler = FindObjectOfType<BlindModeHandler>();
        FindObjectOfType<OVRCameraRig>().rightControllerAnchor.gameObject.AddComponent<ReachGoalTriggerer>();
        OnGoalReached += SpawnNewRoom;
    }

    private void Start() {
        SpawnNewRoom();
        _controllerPanel.SetActive(false);
        _spawnRoomBtn.onClick.AddListener(SpawnNewRoom);
        _blindModeBtn.onClick.AddListener(()=> _blindModeHandler.ToogleBlindMode());
        _encodingPicker.onValueChanged.AddListener(i => OnEncodingChanged?.Invoke(i));
        _fixRoomSizeToogle.isOn = _virtualRoom.UseFixedRoomSize;
        _fixRoomSizeToogle.onValueChanged.AddListener(b => _virtualRoom.UseFixedRoomSize = b);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            ToggleControllerPanel();
        }
    }
    private void ToggleControllerPanel() {
        _controllerPanel.SetActive(!_controllerPanel.activeInHierarchy);
        if (_controllerPanel.activeInHierarchy) {
            _controllerPanel.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.7f;
        }
    }
    public void SpawnNewRoom() {
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
