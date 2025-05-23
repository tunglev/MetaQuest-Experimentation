using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


// EncodingMethod: pitch and tempo
public class RayProjectionHorizontal : EncodingMethod
{
    [SerializeField] private AudioClip _clip;

    private Transform _centerEye;
    private AudioSource m_audioSrc;
    private TriggerEverySeconds _triggerEverySeconds;
    [SerializeField] private bool _isProjecting = false;
    public ConfigInput<int> maxDistance = ConfigInput<int>.IntConfig.Create("Max Distance", 7, 1, 30);

    public override void InitOnCam(GameObject centerEye)
    {
        if (IsInit) return;
        _centerEye = centerEye.transform;
        m_audioSrc = _centerEye.AddComponent<AudioSource>();
        _triggerEverySeconds = _centerEye.AddComponent<TriggerEverySeconds>();
        _triggerEverySeconds.OnTriggered += ToggleAudio;
        InitAudioSource(m_audioSrc);

        void InitAudioSource(AudioSource src)
        {
            src.clip = _clip;
            src.spatialBlend = 0f; // 2D
            src.playOnAwake = false;
            src.panStereo = 0f;
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

    

    private void StartRayProjection()
    {
        _isProjecting = true;
    }

    private void StopRayProjection()
    {
        _isProjecting = false;
    }

    private void ToggleAudio()
    {
        if (m_audioSrc.isPlaying)
        {
            m_audioSrc.Stop();
        }
        else
        {
            m_audioSrc.Play();
        }
    }
    private void Update()
    {
        if (_isProjecting)
        {
            HorizontalRayProjectionFromCenterEye(m_audioSrc, Vector3.forward);
        }
        else
        {
            m_audioSrc.Stop();
        }
    }

    // project ray parallel to the floor
    void HorizontalRayProjectionFromCenterEye(AudioSource source, Vector3 normalizedDir) // ray always cast paralel to the floor
    {
        #if UNITY_EDITOR
            Debug.DrawRay(_centerEye.position, Vector3.ProjectOnPlane(_centerEye.TransformDirection(normalizedDir) * maxDistance.Value, Vector3.up), Color.green);
        #endif
        int maxDistanceValue = maxDistance.Value;
        if (Physics.Raycast(_centerEye.position, Vector3.ProjectOnPlane(_centerEye.TransformDirection(normalizedDir), Vector3.up), out RaycastHit hit, maxDistanceValue))
        {
            Vector3 closestPointOnCollider = hit.collider.ClosestPointOnBounds(_centerEye.position);
            source.pitch = Mathf.Lerp(3, 0, (float) (closestPointOnCollider - _centerEye.position).magnitude / maxDistanceValue);
            //if (!source.isPlaying) source.Play();

            float distanceToHitPoint = (hit.point - _centerEye.position).magnitude;
            _triggerEverySeconds.SetTempo(Mathf.Lerp(0.1f, 2f, distanceToHitPoint / maxDistanceValue));
        }
        else
        {
            source.Stop();
        }
    }
}
