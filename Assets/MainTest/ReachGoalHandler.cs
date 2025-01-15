using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoalHandler : MonoBehaviour
{
    private void Awake() {
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        var c = gameObject.AddComponent<SphereCollider>();
        c.radius = 0.08f;
        c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Equals("GOAL_CLD")) {
            FindObjectOfType<SpawnVirtualRoom>().SPAWN();
        }
    }
}
