using UnityEngine;

public class AlwaysFaceCam : MonoBehaviour
{
    private Transform m_camTransform;

    void Start()
    {
        m_camTransform = Camera.main.transform;
    }
    void Update()
    {
        transform.forward = m_camTransform.forward;
    }
}
