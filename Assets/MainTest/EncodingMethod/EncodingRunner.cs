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
        var allEncodings = GetComponents<EncodingMethod>();
        foreach (var e in allEncodings) {
            if (e == _encodingMethod) continue;
            e.enabled = false;
        }
    }

    private void Update() {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)) // right hand trigger (left hand is PrimaryIndexTrigger)
        {
            _encodingMethod.OnDemandTriggered();
            OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
        }
        if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger)) 
        {
            _encodingMethod.OnDemandTriggeredStop();
        }
        FindObjectOfType<SceneDebugger>().logs.text = Camera.main.transform.position.ToString();
    }

    #if UNITY_EDITOR
    [ContextMenu("Test Trigger")]
    public void TestTrigger() {
        _encodingMethod.OnDemandTriggered();
    }
    #endif
}
