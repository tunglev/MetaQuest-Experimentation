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
            print(other.name);
            isExteriorWall = true;
            var ren = GetComponent<Renderer>();
            var mat = ren.material;
            mat.color = Color.green;
            ren.material = mat;
        }
    }
    void OnTriggerExit(Collider other)
    {
        //if (other.name != "WALL_FACE_EffectMesh" && other.name != "STORAGE_EffectMesh") return;
        //print(isExteriorWall? "exterior walls" : "into the void");
        Destroy(gameObject,1f);
        var ren = GetComponent<Renderer>();
        var mat = ren.material;
        mat.color = Color.blue;
        ren.material = mat;
    }
}
 