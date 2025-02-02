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

    [SerializeField] private Transform cylinderPrefab;
    [SerializeField] private float _maxRadius = 8;
    [SerializeField] private float _initGrowSpd = 0.5f;
    [Header("Audio pin")]
    [SerializeField] private AudioPin _audioPinPrefab;
    [SerializeField] private float _audioPinLifeTime = 3;
    private float _curGrowSpd;
    private Transform _cylinder;


    private void FixedUpdate()
    {
        _cylinder.localScale += _curGrowSpd * Time.fixedDeltaTime * new Vector3(1,0,1);
        if (_cylinder.localScale.x > _maxRadius)
        {
            ResetCylinder();
        }
    }
    protected virtual void HandleTriggerWithAnAnchor(Collider other)
    {
        if (_curGrowSpd == 0f) return;
        if (other.CompareTag("NoSound")) return;
        var contactPoint = other.ClosestPointOnBounds(_cylinder.transform.position);
        var anchor = other.GetComponentInParent<MRUKAnchor>();
        AudioPin pin = Instantiate(_audioPinPrefab, contactPoint, Quaternion.identity);
        var distance = (contactPoint - _cylinder.transform.position).magnitude;
        pin.InitializeDistance(distance);
        if (anchor != null)
        {
            pin.InitializeLabel(anchor.name);
        }

        Destroy(pin.gameObject, _audioPinLifeTime);
    }

    private void StartCylinderGrow()
    {
        ResetCylinder();
        _curGrowSpd = _initGrowSpd;
    }
    public void ResetCylinder()
    {
        _curGrowSpd = 0f;
        _cylinder.localScale = new Vector3(0, _cylinder.localScale.y, 0);
    }

}
