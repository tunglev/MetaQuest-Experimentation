using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private AudioSource audioSrc;

    void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.playOnAwake = false;
        audioSrc.loop = true;
        audioSrc.clip = (AudioClip)Resources.Load("Audio/Goal");
    }
    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            audioSrc.Play();
        }
        if (OVRInput.GetUp(OVRInput.RawButton.A))
        {
            audioSrc.Stop();
        }
    }
}
