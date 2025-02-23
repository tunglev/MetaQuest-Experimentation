using System.Collections.Generic;
using UnityEngine;

public class MovingAudioPin : MonoBehaviour
{
    private bool isInit = false;
    private bool isExteriorWall = false;
    private Vector3 moveDirection;
    private float moveSpeed;

    public void Init(Vector3 moveDirection, float moveSpeed) {
        this.moveDirection = moveDirection;
        this.moveSpeed = moveSpeed;
        isInit = true;
    }

    void Update()
    {
        if (!isInit) return;
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    private Collider currentCollider;

    void OnTriggerEnter(Collider other)
    {
        if (currentCollider == null) currentCollider = other;
        else {
            Destroy(gameObject,1f);
            float disToOther = Vector3.ProjectOnPlane(transform.position - other.transform.position, Vector3.up).magnitude;
            float otherLongSide = Mathf.Max(other.bounds.size.x, other.bounds.size.z) * 0.5f;
            print(disToOther);
            print(otherLongSide);
            if (disToOther < otherLongSide) {
                EndWithClosedWall();
            }
            else {
                EndWithOpenWall();
            }
            
            
        }
    }
    void OnTriggerExit(Collider other)
    {
        EndWithOpenWall();
    }

    void EndWithClosedWall() {
        isExteriorWall = true;
        var ren = GetComponent<Renderer>();
        var mat = ren.material;
        mat.color = Color.green;
        ren.material = mat;
    }
    void EndWithOpenWall() {
        Destroy(gameObject, 1f);
        var ren = GetComponent<Renderer>();
        var mat = ren.material;
        mat.color = Color.blue;
        ren.material = mat;
    } 
}
 