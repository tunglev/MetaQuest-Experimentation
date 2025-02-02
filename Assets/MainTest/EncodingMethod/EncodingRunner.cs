using System;
using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using UnityEngine;

public class EncodingRunner : MonoBehaviour
{
    [SerializeField]
    private EncodingMethod[] _encodingArray;
    private EncodingMethod _currentEncoding;
    private GameObject _centerEye;

    private void Awake() {
        _centerEye = Camera.main.gameObject;
    }

    private void Start() {
        SelectEncodingFromArray(0);
        MainTestHandler.Instance.OnEncodingChanged += SelectEncodingFromArray;
    }

    private void SelectEncodingFromArray(int i) {
        SelectEncodingMethod(_encodingArray[i]);
    }

    private void SelectEncodingMethod(EncodingMethod encoding) {
        encoding.InitOnCam(_centerEye);
        encoding.enabled = true;
        encoding.IsInit = true;
        _currentEncoding = encoding;
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
        _currentEncoding.OnDemandTriggeredDown();
        OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
    }
    public void TriggerUp() {
        _currentEncoding.OnDemandTriggeredUp();
    }
}
