using System.Collections.Generic;
using UnityEngine;

public class MovingAudioPin : MonoBehaviour
{
    private bool isInit = false;
    private bool isExteriorWall = false;
    private Vector3 moveDirection;

    public void Init(Vector3 moveDirection) {
        this.moveDirection = moveDirection;
        isInit = true;
    }

    void Update()
    {
        if (!isInit) return;
        transform.position += this.moveDirection * Time.deltaTime * 0.1f;
    }

    private Collider currentCollider;

    void OnTriggerEnter(Collider other)
    {
        if (currentCollider == null) currentCollider = other;
        else Destroy(gameObject);
        //print("Trigger entered");
        print(other.name);
        isExteriorWall = true;           
    }
    void OnTriggerExit(Collider other)
    {
        //if (other.name != "WALL_FACE_EffectMesh" && other.name != "STORAGE_EffectMesh") return;
        //print(isExteriorWall? "exterior walls" : "into the void");
        Destroy(gameObject);
    }
}
 