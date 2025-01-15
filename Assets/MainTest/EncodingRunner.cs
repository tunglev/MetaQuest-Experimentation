using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncodingRunner : MonoBehaviour
{
    [SerializeField] private EncodingMethod _encodingMethod;

    [Header("Trigger keys")]
    [SerializeField] private OVRInput.Button _OVRButton;


    private void Update() {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)) // right hand trigger (left hand is PrimaryIndexTrigger)
        {
            _encodingMethod.OnDemandTriggered();
            OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
        }
    }

    #if UNITY_EDITOR
    [ContextMenu("Test Trigger")]
    private void TestTrigger() {
        _encodingMethod.OnDemandTriggered();
    }
    #endif
}
