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
    
    private bool _adminMode = true;
    public bool AdminMode  {
        get {
            return _adminMode;
        }
        set {
            _adminMode = value;
            if (_adminMode) {
                // admin mode on
            } else {
                _controllerPanel.gameObject.SetActive(false);
            }
        }
    }
    public Action OnNewRoomSpanwed;
    public Action<bool> OnBlindModeToggled;
    public Action OnGoalReached;
    public Action<int, char> OnEncodingChanged;
    public bool IsBlind {
        get {
            return _blindModeHandler.IsBlind;
        }
    }

    [SerializeField] ControllerPanel _controllerPanel;

    private SpawnVirtualRoom _virtualRoom;
    private BlindModeHandler _blindModeHandler;

    private void Awake()
    {
        if (Instance==null)Instance = this;
        else Destroy(this);
        _virtualRoom = FindObjectOfType<SpawnVirtualRoom>();
        _blindModeHandler = FindObjectOfType<BlindModeHandler>();
        FindObjectOfType<OVRCameraRig>().rightControllerAnchor.gameObject.AddComponent<ReachGoalTriggerer>();
        OnGoalReached += ()=>TestSubjectHandler.Instance.SpawnNextPanel();
    }

    private void Start() {
        SpawnNewRoom();
        _controllerPanel.gameObject.SetActive(false);
        _controllerPanel.spawnRoomButton.onClick.AddListener(SpawnNewRoom);
        _controllerPanel.blindModeButton.onClick.AddListener(()=> _blindModeHandler.ToogleBlindMode());
        _controllerPanel.toogleUseFixRoomSize.isOn = _virtualRoom.UseFixedRoomSize;
        _controllerPanel.toogleUseFixRoomSize.onValueChanged.AddListener(b => _virtualRoom.UseFixedRoomSize = b);

        //encoding 0 - global, 1 - specialized
        _controllerPanel.globalEncodingDropdown.onValueChanged.AddListener(i => OnEncodingChanged?.Invoke(i, 'g'));
        _controllerPanel.specializedEncodingDropdown.onValueChanged.AddListener(i => OnEncodingChanged?.Invoke(i, 's'));
    }

    private void Update()
    {
        if (!AdminMode) return;
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            ToggleControllerPanel();
        }
    }
    private void ToggleControllerPanel() {
        _controllerPanel.gameObject.SetActive(!_controllerPanel.gameObject.activeInHierarchy);
        if (_controllerPanel.gameObject.activeSelf) {
            _controllerPanel.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 1f;
            _controllerPanel.transform.forward = Camera.main.transform.forward;
        }
    }
    public void SpawnNewRoom() {
        _virtualRoom.SpawnNewRoomAsMRUKRoom();
        _blindModeHandler.ReapplyBlindMode();
        OnNewRoomSpanwed?.Invoke();
    }
    public bool ToggleBlindMode() {
        _blindModeHandler.ToogleBlindMode();
        OnBlindModeToggled?.Invoke(_blindModeHandler.IsBlind);
        return _blindModeHandler.IsBlind;
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        var myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 50;
        GUILayout.BeginArea(new Rect(10, 10, 300, 900));

        if (GUILayout.Button("SpawnNewRoom")) SpawnNewRoom();
        if (GUILayout.Button("trigger global down")) FindObjectOfType<EncodingRunner>().TriggerDownGlobal();
        if (GUILayout.Button("trigger global up")) FindObjectOfType<EncodingRunner>().TriggerUpGlobal();
        if (GUILayout.Button("trigger specialized down")) FindObjectOfType<EncodingRunner>().TriggerDownSpecialized();
        if (GUILayout.Button("trigger specialized up")) FindObjectOfType<EncodingRunner>().TriggerUpSpecialized();
        if (GUILayout.Button("Toggle BlindMode"))
        {
            _blindModeHandler.ToogleBlindMode();
        }

        GUILayout.EndArea();
    }
#endif
}
