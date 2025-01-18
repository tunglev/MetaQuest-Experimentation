using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class BlindModeHandler : MonoBehaviour
{
    [SerializeField] private float _visibleRange = 0.15f;
    [SerializeField] private Material _blindModeMaterial;

    [SerializeField] private EffectMesh _roomGuardianEffectMesh;
    [SerializeField] private EffectMesh _roomBoxEffectMesh;
    private Material _roomBoxMaterial;

    private bool _blind = false;

    void Start()
    {
        _roomBoxMaterial = _roomBoxEffectMesh.MeshMaterial;  
        FindObjectOfType<SphereMaskController>().softness = _visibleRange;  
    }
    private void Update() {
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            ToogleBlindMode();
        }
    }

    [ContextMenu("Toggle BlindMode")]
    public void ToogleBlindMode() {
        SetBlindMode(!_blind);
    }

    public void SetBlindMode(bool val) {
        FindObjectOfType<SpawnVirtualRoom>().CurrentGoal.GetComponentInChildren<MeshRenderer>().enabled = !val;
        _roomGuardianEffectMesh.ToggleEffectMeshVisibility(shouldShow: !val);
        _roomBoxEffectMesh.OverrideEffectMaterial(val ? _blindModeMaterial : _roomBoxMaterial);
        _blind = val;
    }

    public void ReapplyBlindMode() {
        SetBlindMode(_blind);
    }
}
