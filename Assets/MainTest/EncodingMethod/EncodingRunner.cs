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

    private void Start() {
        var centereye = Camera.main.gameObject;
        print(centereye.name);
        _encodingMethod.InitOnCam(centereye);
        _encodingMethod.enabled = true;
    }

    private void Update() {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)) // right hand trigger (left hand is PrimaryIndexTrigger)
        {
            TriggerDown();
        }
        if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger)) 
        {
            TriggerUp();
        }
    }

    public void TriggerDown() {
        _encodingMethod.OnDemandTriggeredDown();
        OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
    }
    public void TriggerUp() {
        _encodingMethod.OnDemandTriggeredUp();
    }
}
