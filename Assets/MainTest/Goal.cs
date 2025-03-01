using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(Renderer))]
public class Goal : MonoBehaviour
{
    private AudioSource m_audioSrc;
    private Renderer m_renderer;

    void Awake()
    {
        m_audioSrc = GetComponent<AudioSource>();
        m_renderer = GetComponent<Renderer>();
        SetInvisibility(MainTestHandler.Instance.IsBlind);
        MainTestHandler.Instance.OnBlindModeToggled += SetInvisibility;
        m_audioSrc.playOnAwake = false;
        m_audioSrc.loop = true;
        m_audioSrc.clip = (AudioClip)Resources.Load("Audio/Goal");
    }

    private void SetInvisibility(bool isInvisible) {
        m_renderer.enabled = !isInvisible;
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
