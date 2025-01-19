using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    public static GlobalAudio Instance {get; private set;}

    private AudioSource _audioSource;
    private string _currentlyPlaying;

    private void Awake() {
        if (Instance == null) Instance = this;
        else {
            Destroy(this);
        }
    }

    private void Start() {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.spatialBlend = 0f; // 2D sound
        _audioSource.playOnAwake = false;
    }


    public void PlaySound(string soundName, bool loop = false) {
        AudioClip audioClip = (AudioClip) Resources.Load($"Audio/{soundName}");
        _audioSource.loop = loop;
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }

    public void StopSound(string soundName) {
        if (_currentlyPlaying != soundName) return;
        StopSound();
    }
    public void StopSound() {
        _audioSource.Stop();
    }
}
