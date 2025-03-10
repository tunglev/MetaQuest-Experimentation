using System.Collections;
using System.Collections.Generic;
using Meta.XR.MRUtilityKit;
using UnityEngine;


public class SphereGrow : EncodingMethod
{
    public override void OnDemandTriggeredDown()
    {
        StartSphereGrow();
    }
    public override void InitOnCam(GameObject centerEye)
    {
        if (IsInit) return;
        var rb = centerEye.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        _sphereCollider = centerEye.AddComponent<SphereCollider>();
        _sphereCollider.isTrigger = true;
        centerEye.AddComponent<ColliderUtilities>().onTriggerEnter += HandleTriggerWithAnAnchor;
        MainTestHandler.Instance.OnNewRoomSpanwed += ResetSphere;

        ResetSphere();
    } 

    [SerializeField] private float _maxRadius = 8;
    [SerializeField] private float _initGrowSpd = 0.5f;
    [Header("Audio pin")]
    [SerializeField] private AudioPin _audioPinPrefab;
    [SerializeField] private float _audioPinLifeTime = 3;
    private float _curGrowSpd;
    private SphereCollider _sphereCollider;


    private void FixedUpdate()
    {
        _sphereCollider.radius += Time.fixedDeltaTime * _curGrowSpd;
        if (_sphereCollider.radius > _maxRadius)
        {
            ResetSphere();
        }
    }
    private void HandleTriggerWithAnAnchor(Collider other)
    {
        if (_curGrowSpd == 0f) return;
        if (other.CompareTag("NoSound")) return;
        var contactPoint = other.ClosestPointOnBounds(_sphereCollider.transform.position);
        var anchor = other.GetComponentInParent<MRUKAnchor>();
        AudioPin pin = Instantiate(_audioPinPrefab, contactPoint, Quaternion.identity);
        var distance = (contactPoint - _sphereCollider.transform.position).magnitude;
        pin.InitializeDistance(distance);
        if (anchor != null)
        {
            pin.InitializeLabel(anchor.name);
        }

        Destroy(pin.gameObject, _audioPinLifeTime);
    }

    private void StartSphereGrow()
    {
        ResetSphere();
        _curGrowSpd = _initGrowSpd;
    }
    public void ResetSphere()
    {
        _curGrowSpd = 0f;
        _sphereCollider.radius = 0f;
    }

}
