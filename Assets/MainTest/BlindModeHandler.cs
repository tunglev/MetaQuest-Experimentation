using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;

public class BlindModeHandler : MonoBehaviour
{
    public bool IsBlind = false;
    [SerializeField] private float _visibleRange = 0.15f;
    [SerializeField] private Material _blindModeMaterial;

    [SerializeField] private EffectMesh _roomGuardianEffectMesh;
    [SerializeField] private EffectMesh _roomBoxEffectMesh;
    private Material _roomBoxMaterial;


    void Start()
    {
        _roomBoxMaterial = _roomBoxEffectMesh.MeshMaterial;  
        FindObjectOfType<SphereMaskController>().softness = _visibleRange;  
    }

    [ContextMenu("Toggle BlindMode")]
    public void ToogleBlindMode() {
        SetBlindMode(!IsBlind);
        GlobalAudio.Instance.PlaySound(IsBlind ? "Blind mode - on" : "Blind mode - off");
    }

    public void SetBlindMode(bool val) {
        FindObjectOfType<SpawnVirtualRoom>().CurrentGoal.GetComponentInChildren<MeshRenderer>().enabled = !val;
        _roomGuardianEffectMesh.ToggleEffectMeshVisibility(shouldShow: !val);
        _roomBoxEffectMesh.OverrideEffectMaterial(val ? _blindModeMaterial : _roomBoxMaterial);
        IsBlind = val;
    }

    public void ReapplyBlindMode() {
        SetBlindMode(IsBlind);
    }
}
