using Meta.XR.MRUtilityKit;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPin : MonoBehaviour
{
    public AudioClip defaultClip;
    public List<LabelAudioMap> map;
    public void Initialize(MRUKAnchor.SceneLabels label)
    {
        var src = GetComponent<AudioSource>();
        var target = map.Find(e => e.label == label);
        if (target != null)
        {
            src.clip = target.audioClip;
        }
        else
        {
            src.clip = defaultClip;
        }
        src.Play();
    }
}

[System.Serializable]
public class LabelAudioMap {
    public MRUKAnchor.SceneLabels label;
    public AudioClip audioClip;
}

