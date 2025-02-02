using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RayProjection : EncodingMethod
{
    public override void InitOnCam(GameObject centerEye)
    {
        if (IsInit) return;
        _centerEye = centerEye.transform;
        _rightEarAudio = _centerEye.AddComponent<AudioSource>();
        _leftEarAudio = _centerEye.AddComponent<AudioSource>();
        InitAudioSource(_rightEarAudio, isRight: true);
        InitAudioSource(_leftEarAudio, isRight: false);

        void InitAudioSource(AudioSource src, bool isRight) {
            src.clip = _clip;
            src.spatialBlend = 0f; // 2D
            src.playOnAwake = false;
            src.panStereo = isRight? 1f : -1f;
            src.loop = true;
        }
    }
    public override void OnDemandTriggeredDown()
    {
        StartRayProjection();
    }
    public override void OnDemandTriggeredUp()
    {
        StopRayProjection();
    }

    [SerializeField] private AudioClip _clip;

    private Transform _centerEye;
    private AudioSource _rightEarAudio;
    private AudioSource _leftEarAudio;
    [SerializeField]private bool _isProjecting = false;


    private void StartRayProjection() 
    {
        _isProjecting = true;
    }

    private void StopRayProjection()
    {
        _isProjecting = false;
    }

    private void Update() {
        if (_isProjecting) {
            RayProjectionFromCenterEye(_rightEarAudio, Vector3.right);
            RayProjectionFromCenterEye(_leftEarAudio, Vector3.left);
        }
        else {
            _rightEarAudio.Stop();
            _leftEarAudio.Stop();
        }
    }

    void RayProjectionFromCenterEye(AudioSource source, Vector3 direction) {
        if (Physics.Raycast(_centerEye.position, _centerEye.TransformDirection(direction), out RaycastHit hit, Mathf.Infinity))
        {
            source.pitch = Mathf.Lerp(-3, 3, (float)hit.distance / 5f);
            if (!source.isPlaying) source.Play();
        }
        else
        {
            source.Stop();
        }
    }
}
