using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class SphereCastHandler : MonoBehaviour
{
    public float growthSpd;
    private SphereCollider m_sphere;

    private void Awake()
    {
        m_sphere = GetComponent<SphereCollider>();
    }
    private void FixedUpdate()
    {
        m_sphere.radius += Time.deltaTime * growthSpd;
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
    }
}
