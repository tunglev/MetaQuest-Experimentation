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
    [SerializeField] private ConfigInput<float> _initGrowSpd = ConfigInput<float>.FloatConfig.Create("Grow Speed", 0.5f, 0f, 20f);
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
        _curGrowSpd = _initGrowSpd.Value;
    }
    public void ResetSphere()
    {
        _curGrowSpd = 0f;
        _sphereCollider.radius = 0f;
    }

    private bool IsWithinCameraViewAngle(Vector3 point, Transform camera, float angleDeg)
    {
        if (0f <= angleDeg && angleDeg <= 180f)
        {
            Vector3 directionToPoint = point - camera.position;
            Vector3 projectedDirectionToPoint = Vector3.ProjectOnPlane(directionToPoint, Vector3.up);
            Vector3 projectedCameraForward = Vector3.ProjectOnPlane(camera.forward, Vector3.up);
            float angleToPoint = Vector3.Angle(projectedCameraForward, projectedDirectionToPoint);
            return angleToPoint <= angleDeg / 2f;
        }
        else
        {
            Debug.LogError("Invalid angle: " + angleDeg + ". Angle must be between 0 and 180 degrees.");
            return false;
        }
        
    }

}
