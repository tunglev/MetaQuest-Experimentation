using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoalTriggerer : MonoBehaviour
{
    private Transform handTransform;
    private Transform cylinderTransform;
    private void Awake() {
        handTransform = transform;
        GameObject temp = new("Hand Cylinder")
        {
            layer = LayerMask.NameToLayer("Human"),
            tag = "NoSound"
        };
        cylinderTransform = temp.transform;
        var rb = temp.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        var c = temp.AddComponent<CapsuleCollider>();
        var cylinderUtils = temp.AddComponent<ColliderUtilities>();
        cylinderUtils.onTriggerEnter += OnTriggerEnter;
        c.radius = 0.08f;
        c.height = 5;
        c.isTrigger = true;
        
    }


    private void Update()
    {
        cylinderTransform.position = handTransform.position;
    }
    private void OnTriggerEnter(Collider other) {
        print(other.gameObject.name);
        if (other.gameObject.name.Equals("GOAL_CLD")) {
            OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
            GlobalAudio.Instance.PlaySound("Success");
            Destroy(other.transform.parent.gameObject);
            MainTestHandler.Instance.OnGoalReached?.Invoke();
        }
    }
}
