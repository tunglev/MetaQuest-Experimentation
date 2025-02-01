using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RayProjection : EncodingMethod
{
    public override void InitOnCam(GameObject centerEye)
    {
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
            if (Physics.Raycast(_centerEye.position, _centerEye.right, out RaycastHit rightHit, Mathf.Infinity))
            {
                _rightEarAudio.pitch = Mathf.Lerp(-3, 3, (float)rightHit.distance / 5f);
                if (!_rightEarAudio.isPlaying) _rightEarAudio.Play();
            }
            else
            {
                _rightEarAudio.Stop();
            }
        }
        else {
            _rightEarAudio.Stop();
            _leftEarAudio.Stop();
        }
    }
}
