using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoalTriggerer : MonoBehaviour
{
    
    private void Awake() {
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        var c = gameObject.AddComponent<SphereCollider>();
        c.radius = 0.08f;
        c.isTrigger = true;
        tag = "NoSound";
        
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Equals("GOAL_CLD")) {
            OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
            GlobalAudio.Instance.PlaySound("Success");
            Destroy(other.transform.parent.gameObject);
            MainTestHandler.Instance.OnGoalReached?.Invoke();
        }
    }
}
