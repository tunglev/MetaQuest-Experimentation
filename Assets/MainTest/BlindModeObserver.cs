using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BlindModeObserver : MonoBehaviour
{
    private Renderer m_renderer;
    void Awake()
    {
        m_renderer = GetComponent<Renderer>();
        if (m_renderer == null) throw new System.Exception("BlindModeObserver has to attached with a renderer component.");
    }
    void OnEnable()
    {
        SetInvisiblity(MainTestHandler.Instance.IsBlind);
    }
    void Start()
    {
        MainTestHandler.Instance.OnBlindModeToggled += SetInvisiblity;
    }
    void OnDestroy()
    {
        MainTestHandler.Instance.OnBlindModeToggled -= SetInvisiblity;
    }

    private void SetInvisiblity(bool val) {
        m_renderer.enabled = !val;
    }
    
}
