using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using UnityEngine;

public class EncodingRunner : MonoBehaviour
{
    [SerializeField]
    private EncodingMethod _encodingMethod;

    private void Awake() {
        var centereye = Camera.main.gameObject;
        _encodingMethod.InitOnCam(centereye);
    }

    private void Update() {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)) // right hand trigger (left hand is PrimaryIndexTrigger)
        {
            _encodingMethod.OnDemandTriggered();
            OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
        }
        FindObjectOfType<SceneDebugger>().logs.text = Camera.main.transform.position.ToString();
    }

    #if UNITY_EDITOR
    [ContextMenu("Test Trigger")]
    private void TestTrigger() {
        _encodingMethod.OnDemandTriggered();
    }
    #endif
}
