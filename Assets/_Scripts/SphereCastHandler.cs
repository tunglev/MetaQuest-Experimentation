using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class SphereCastHandler : MonoBehaviour
{
    private float growthSpd;
    public AudioPin audioPinPrefab;
    private SphereCollider m_sphere;

    private void Awake()
    {
        m_sphere = GetComponent<SphereCollider>();
        m_sphere.isTrigger= true;
        resetSphere();
    }
    private void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.One))
        {
            StartSphereGrow();
        }
    }

    
    private void FixedUpdate()
    {
        m_sphere.radius += Time.deltaTime * growthSpd;
        if (m_sphere.radius > 8)
        {
            resetSphere();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        var contactPoint = other.ClosestPointOnBounds(this.transform.position);
        var anchor = other.GetComponentInParent<MRUKAnchor>();
        AudioPin pin = Instantiate(audioPinPrefab, contactPoint, Quaternion.identity);
        var distance = (contactPoint - this.transform.position).magnitude;
        pin.InitializeDistance(distance);
        if (anchor!= null)
        {
            pin.InitializeLabel(anchor.GetLabelsAsEnum());
        }

        Destroy(pin.gameObject, 3);
    }

    [ContextMenu("StartSphereGrow")]
    public void StartSphereGrow()
    {
        resetSphere();
        growthSpd = 0.5f;
    }
    private void resetSphere()
    {
        //reset
        growthSpd = 0f;
        m_sphere.radius = 0f;
    }
}
