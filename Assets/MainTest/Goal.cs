using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Goal : MonoBehaviour
{
    private AudioSource m_audioSrc;

    void Awake()
    {
        m_audioSrc = GetComponent<AudioSource>();
        m_audioSrc.playOnAwake = false;
        m_audioSrc.loop = true;
        m_audioSrc.clip = (AudioClip)Resources.Load("Audio/Goal");
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            m_audioSrc.Play();
        }
        if (OVRInput.GetUp(OVRInput.RawButton.A))
        {
            m_audioSrc.Stop();
        }
    }
}
