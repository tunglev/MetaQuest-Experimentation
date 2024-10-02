using Meta.XR.MRUtilityKit;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPin : MonoBehaviour
{
    public AudioClip defaultClip;
    public List<LabelAudioMap> labelMap;
    public AnimationCurve distance_frequencyMap;
    private AudioSource m_src;

    private void OnValidate()
    {
        foreach(var key in distance_frequencyMap.keys)
        {
            if (Mathf.Abs(key.value) > 3) Debug.LogWarning("key value should be in range -3 to 3 (for unity AudioSource pitch)");
            if (key.time < 0) Debug.LogWarning("key time (distance) doesn't have negative meaning");
        }
    }

    private void Awake()
    {
        m_src = GetComponent<AudioSource>();
    }
    public void InitializeLabel(MRUKAnchor.SceneLabels label)
    {
        var target = labelMap.Find(e => e.label == label);
        if (target != null)
        {
            m_src.clip = target.audioClip;
            m_src.volume = target.volume;
        }
        else
        {
            m_src.clip = defaultClip;
        }
        m_src.Play();
    }
    public void InitializeDistance(float distance)
    {
        m_src.pitch = distance_frequencyMap.Evaluate(distance);
    }
}
//
[System.Serializable]
public class LabelAudioMap {
    public MRUKAnchor.SceneLabels label;
    public AudioClip audioClip;
    [Range(0, 1)] public float volume =1f;
}

