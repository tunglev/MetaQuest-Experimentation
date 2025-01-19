using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Meta.XR.MRUtilityKit;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WallAlertHandler : MonoBehaviour
{
    [SerializeField] private float _alertRadius;

    private Transform _centerEye;
    private AudioSource _audioSource;
    private LayerMask _maskIgnoreHumanLayer;
    void Start()
    {
        _centerEye =  FindObjectOfType<OVRCameraRig>().centerEyeAnchor;
        _audioSource = GetComponent<AudioSource>();

        int layer = LayerMask.NameToLayer("Human");
        _maskIgnoreHumanLayer =  ~(1 << layer);
    }

    // Update is called once per frame
    void Update()
    {
        bool isInRoom = MRUK.Instance.GetCurrentRoom().IsPositionInRoom(_centerEye.position);
        var colliders = Physics.OverlapSphere(_centerEye.position, _alertRadius, _maskIgnoreHumanLayer);
        if (colliders.Length == 0 && isInRoom) {
            _audioSource.Stop();
        }
        else {
            if (!_audioSource.isPlaying) _audioSource.Play();
        }
    }
}
