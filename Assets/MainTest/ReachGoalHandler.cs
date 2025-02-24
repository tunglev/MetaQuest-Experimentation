using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoalHandler : MonoBehaviour
{
    private AudioSource audioSrc;
    
    private void Awake() {
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        var c = gameObject.AddComponent<SphereCollider>();
        c.radius = 0.08f;
        c.isTrigger = true;
        tag = "NoSound";
        audioSrc = GetComponent<AudioSource>();
        audioSrc.playOnAwake = false;
        audioSrc.loop = true;
        audioSrc.clip = (AudioClip)Resources.Load("Audio/Goal");
    }

    private void Update() {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            audioSrc.Play();
        }
        if (OVRInput.GetUp(OVRInput.RawButton.A))
        {
            audioSrc.Stop();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.name.Equals("GOAL_CLD")) {
            FindObjectOfType<SpawnVirtualRoom>().SpawnNewRoomAsMRUKRoom();
            OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
            GlobalAudio.Instance.PlaySound("Success");
            MainTestHandler.Instance.OnGoalReached?.Invoke();
        }
    }
}
