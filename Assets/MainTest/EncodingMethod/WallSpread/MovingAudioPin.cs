using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingAudioPin : MonoBehaviour
{
    const float WALL_THICKNESS = 0.15f;
    [Serializable]
    public struct AudioSettings {
        public AudioClip clip;
        [Range(0f,1f)] public float volume;
    }
    [Header("Audio clips")]
    public AudioSettings movingSound;
    public AudioSettings openWallSound;
    public AudioSettings closedWallSound;

    private bool isInit = false;
    private Vector3 moveDirection;
    private float moveSpeed;
    private AudioSource audioSrc;

    public void Init(Vector3 moveDirection, float moveSpeed) {
        this.moveDirection = moveDirection;
        this.moveSpeed = moveSpeed;
        isInit = true;
        audioSrc = GetComponent<AudioSource>();
        audioSrc.loop = true;
        audioSrc.clip = movingSound.clip;
        audioSrc.volume = movingSound.volume;
        audioSrc.Play();

        if (MainTestHandler.Instance.IsBlind) {
            GetComponent<Renderer>().enabled = false;
        }
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
            if (!IsAtCurrrentColliderEnd()) return;
            float projectedDistanceToOtherCollder = Vector3.ProjectOnPlane(transform.position - other.transform.position, Vector3.up).magnitude;
            float otherColliderLongerSide = Mathf.Max(other.bounds.size.x, other.bounds.size.z) * 0.5f;
            print(projectedDistanceToOtherCollder);
            print(otherColliderLongerSide);
            if (projectedDistanceToOtherCollder < otherColliderLongerSide) {
                EndWithClosedWall();
            }
            else {
                EndWithOpenWall();
            }
            
            
        }

    }

    bool IsAtCurrrentColliderEnd()
    {
        if (currentCollider.name == "WALL_FACE_EffectMesh" ) return true; // outer walls can stop whenever meet a new collider
        float projectedDistanceToCurrentCollider = Vector3.ProjectOnPlane(transform.position - currentCollider.transform.position, Vector3.up).magnitude;
        float currentColliderLongerSide = Mathf.Max(currentCollider.bounds.size.x, currentCollider.bounds.size.z) * 0.5f;
        return projectedDistanceToCurrentCollider >= currentColliderLongerSide - 0.5f;
    }

    void OnTriggerExit(Collider other)
    {
        if (!IsAtCurrrentColliderEnd()) return;
        EndWithOpenWall();
    }

    void EndWithClosedWall() {
        Destroy(gameObject, 1f);
        moveSpeed = 0f;
        var ren = GetComponent<Renderer>();
        var mat = ren.material;
        mat.color = Color.green;
        ren.material = mat;
        audioSrc.Stop();
        audioSrc.clip = closedWallSound.clip;
        audioSrc.volume = closedWallSound.volume;
        audioSrc.loop = false;
        audioSrc.Play();
    }
    void EndWithOpenWall() {
        if (currentCollider.name == "WALL_FACE_EffectMesh") {
            EndWithClosedWall();
            return;
        }
            Destroy(gameObject, 1f);
        moveSpeed = 0f;
        var ren = GetComponent<Renderer>();
        var mat = ren.material;
        mat.color = Color.blue;
        ren.material = mat;
        audioSrc.Stop();
        audioSrc.clip = openWallSound.clip;
        audioSrc.volume = openWallSound.volume;
        audioSrc.loop = false;
        audioSrc.Play();
    } 
}
 
