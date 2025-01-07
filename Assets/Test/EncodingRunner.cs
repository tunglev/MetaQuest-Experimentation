using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EncodingMethod))]
public class EncodingRunner : MonoBehaviour
{
    [SerializeField] private EncodingMethod _encodingMethod;

    [Header("Trigger keys")]
    [SerializeField] private OVRInput.Button _OVRButton;

 

    private void Update() {
        if (OVRInput.GetDown(_OVRButton)) {
            _encodingMethod.OnDemandTriggered();
        }
    }

    #if UNITY_EDITOR
    [ContextMenu("Test Trigger")]
    private void TestTrigger() {
        _encodingMethod.OnDemandTriggered();
    }
    #endif
}
