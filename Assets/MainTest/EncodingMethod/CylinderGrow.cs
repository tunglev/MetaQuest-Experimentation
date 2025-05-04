using System.Collections.Generic;
using System.Linq;
using Meta.XR.MRUtilityKit;
using UnityEngine;


public class CylinderGrow : EncodingMethod
{
    public override void OnDemandTriggeredDown()
    {
        StartCylinderGrow();
    }
    public override void InitOnCam(GameObject centerEye)
    {
        if (IsInit) return;
        _cylinder = Instantiate(cylinderPrefab, centerEye.transform);
        int humanlayer = LayerMask.NameToLayer("Human");
        _cylinder.gameObject.layer = humanlayer;
        _cylinder.gameObject.AddComponent<Rigidbody>().isKinematic = true;
        _cylinder.gameObject.AddComponent<ColliderUtilities>().onTriggerEnter += HandleTriggerWithAnAnchor;
        MainTestHandler.Instance.OnNewRoomSpanwed += ResetCylinder;

        ResetCylinder();
    } 

    public ConfigInput<int> detectableAngle = ConfigInput<int>.IntConfig.Create("Detectable Angle", 220, 0, 360);
    [SerializeField] private Transform cylinderPrefab;
    [SerializeField] private float _maxRadius = 8;
    public ConfigInput<float> _initGrowSpd = ConfigInput<float>.FloatConfig.Create("Grow Speed", 0.5f, 0f, 20f);
    [Header("Audio pin")]
    [SerializeField] private AudioPin _audioPinPrefab;
    [SerializeField] private float _audioPinLifeTime = 3;
    protected float _curGrowSpd;
    private Transform _cylinder;


    private void FixedUpdate()
    {
        _cylinder.localScale += _curGrowSpd * Time.fixedDeltaTime * new Vector3(1,0,1);
        if (_cylinder.localScale.x > _maxRadius)
        {
            ResetCylinder();
        }
    }

    const int DEFAULT_LAYER_ONLY_MASK = 1 << 0; // default layer is 0
    private readonly ConfigInput<bool> IgnoreObstructed = ConfigInput<bool>.BoolConfig.Create("Ignore Obstructed", true); // no collider behind walls
    private readonly HashSet<int> usedColliderSet = new();

    private void HandleTriggerWithAnAnchor(Collider other)
    {
        if (_curGrowSpd == 0f) return;
        if (other.CompareTag("NoSound")) return;
        var contactPoint = other.ClosestPointOnBounds(_cylinder.transform.position);
        if (!IsWithinCameraViewAngle(contactPoint, Camera.main.transform, detectableAngle.Value)) return;
        Vector3 camPos = Camera.main.transform.position;
        Vector3 eyeToContactPoint = contactPoint - camPos;
        bool isCorner = false;
        if (IgnoreObstructed.Value && Physics.Raycast(camPos, eyeToContactPoint, eyeToContactPoint.magnitude - 0.05f, DEFAULT_LAYER_ONLY_MASK, QueryTriggerInteraction.Ignore)) 
        {
            return;
        }
        if (IgnoreObstructed.Value) {
            var hits = Physics.RaycastAll(camPos, eyeToContactPoint, maxDistance: eyeToContactPoint.magnitude + 0.15f, layerMask: DEFAULT_LAYER_ONLY_MASK, QueryTriggerInteraction.Ignore);
            if (hits.Length != 1) isCorner = true;
            foreach (var hit in hits) {
                if (usedColliderSet.Contains(hit.colliderInstanceID)) return;
                usedColliderSet.Add(hit.colliderInstanceID);
            }
        }
        
        var anchor = other.GetComponentInParent<MRUKAnchor>();
        AudioPin pin = Instantiate(_audioPinPrefab, contactPoint, Quaternion.identity);
        var distance = (contactPoint - _cylinder.transform.position).magnitude;
        pin.InitializeDistance(distance);
        if (anchor != null)
        {
            pin.InitializeLabel(isCorner? "CORNER" : anchor.name);
            OnTriggeredWithAnchor(anchor, contactPoint, other);
        }

        Destroy(pin.gameObject, _audioPinLifeTime);
    }

    protected virtual void OnTriggeredWithAnchor(MRUKAnchor anchor, Vector3 contactPoint, Collider collider) {}

    private void StartCylinderGrow()
    {
        ResetCylinder();
        _curGrowSpd = _initGrowSpd.Value;
    }
    public void ResetCylinder()
    {
        _curGrowSpd = 0f;
        _cylinder.localScale = new Vector3(0, _cylinder.localScale.y, 0);
        usedColliderSet.Clear();
    }

    private bool IsWithinCameraViewAngle(Vector3 point, Transform camera, float angleDeg)
    {
        if (0f <= angleDeg && angleDeg <= 360f)
        {
            Vector3 directionToPoint = point - camera.position;
            Vector3 projectedDirectionToPoint = Vector3.ProjectOnPlane(directionToPoint, Vector3.up);
            Vector3 projectedCameraForward = Vector3.ProjectOnPlane(camera.forward, Vector3.up);
            float angleToPoint = Vector3.Angle(projectedCameraForward, projectedDirectionToPoint);
            print(angleToPoint);
            return angleToPoint <= angleDeg / 2f;
        }
        else
        {
            Debug.LogError("Invalid angle: " + angleDeg + ". Angle must be between 0 and 360 degrees.");
            return false;
        }

    }

}
