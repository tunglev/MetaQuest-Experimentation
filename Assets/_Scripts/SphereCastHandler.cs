using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SphereCastHandler : MonoBehaviour
{
    private float growthSpd;
    public GameObject contactPointPrefab;
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
        var point = Instantiate(contactPointPrefab, contactPoint, Quaternion.identity);
        Destroy(point, 3);
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
