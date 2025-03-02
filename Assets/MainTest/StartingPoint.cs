using System;
using System.Collections;
using UnityEngine;


public class StartingPoint : MonoBehaviour
{
    public Action OnReadyToStart;
    [SerializeField] float waitDuration;
    private float sqrDetectRadius;
    private Transform m_camera;
    private SpriteRenderer m_spriteRen;
    void Awake()
    {
        var detectRadius = transform.lossyScale.x;
        sqrDetectRadius = detectRadius * detectRadius;
        m_camera = Camera.main.transform;
        m_spriteRen = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        m_spriteRen.color = Color.blue;
    }

    bool isWaiting = false;
    void Update()
    {
        if (Vector3.ProjectOnPlane(transform.position - m_camera.position, Vector3.up).sqrMagnitude < sqrDetectRadius) {
            if (!isWaiting) {
                StartCoroutine(WaitDurationCoroutine());
                isWaiting = true;
            }
        }
        else {
            if (isWaiting) {
                StopAllCoroutines();
                m_spriteRen.color = Color.blue;
                isWaiting = false;
            }
        }
    }

    IEnumerator WaitDurationCoroutine() {
        m_spriteRen.color = Color.yellow;
        yield return new WaitForSeconds(waitDuration);
        OnReadyToStart?.Invoke();
        m_spriteRen.color = Color.green;
        isWaiting = false;
    }
}
