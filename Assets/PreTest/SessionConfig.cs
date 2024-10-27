using System;
using UnityEngine;

[Serializable]
public class SessionConfig
{
    public Action onAnyValueChanged;
    [Serializable]
    public struct RoundCount {
        public int audio_n_visual;
        public int audio_only;
        public int visual_only;
    }
    public RoundCount roundCount;
    public Vector2 radiusRange = new(2,2);
    public bool threeD = true;
    public AudioClip audioFile;

}
