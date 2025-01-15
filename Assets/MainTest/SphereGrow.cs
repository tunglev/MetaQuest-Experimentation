using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;


public class SphereGrow : EncodingMethod
{
    public override void OnDemandTriggered()
    {
        StartSphereGrow();
    }

    [SerializeField] private float _maxRadius = 8;
    [SerializeField] private float _initGrowSpd = 0.5f;
    [Header("Audio pin")]
    [SerializeField] private AudioPin _audioPinPrefab;
    [SerializeField] private float _audioPinLifeTime = 3;
    private float _curGrowSpd;
    private SphereCollider _sphereCollider;

    private void Awake()
    {
        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        _sphereCollider = gameObject.AddComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
        ResetSphere();
    }

    private void FixedUpdate()
    {
        _sphereCollider.radius += Time.fixedDeltaTime * _curGrowSpd;
        if (_sphereCollider.radius > _maxRadius)
        {
            ResetSphere();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_curGrowSpd == 0f) return;
        var contactPoint = other.ClosestPointOnBounds(this.transform.position);
        var anchor = other.GetComponentInParent<MRUKAnchor>();
        AudioPin pin = Instantiate(_audioPinPrefab, contactPoint, Quaternion.identity);
        var distance = (contactPoint - this.transform.position).magnitude;
        pin.InitializeDistance(distance);
        if (anchor != null)
        {
            pin.InitializeLabel(anchor.Label);
        }

        Destroy(pin.gameObject, _audioPinLifeTime);
    }

    private void StartSphereGrow()
    {
        ResetSphere();
        _curGrowSpd = _initGrowSpd;
    }
    private void ResetSphere()
    {
        _curGrowSpd = 0f;
        _sphereCollider.radius = 0f;
    }

}
