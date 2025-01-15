using System;
using UnityEngine;

public class ColliderUtilities : MonoBehaviour
{
    public Action<Collider> onTriggerEnter;
    public Action<Collider> onTriggerExit;
    public Action<Collider> onTriggerStay;
    public Action<Collision> onCollisionEnter;
    public Action<Collision> onCollisionExit;
    public Action<Collision> onCollisionStay;

    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnter?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExit?.Invoke(other);
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStay?.Invoke(other);
    }

    private void OnCollisionEnter(Collision other)
    {
        onCollisionEnter?.Invoke(other);
    }

    private void OnCollisionExit(Collision other)
    {
        onCollisionExit?.Invoke(other);
    }

    private void OnCollisionStay(Collision other)
    {
        onCollisionStay?.Invoke(other);
    }
}
