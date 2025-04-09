using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RoomMover : MonoBehaviour
{
    public static RoomMover Instance {get; private set;}
    public Transform target; // The object to move and rotate
    public float positionSpeed = 0.1f; // Speed of position change on XZ plane
    public float rotationSpeed = 30.0f; // Degrees per second for Y-axis rotation
    
    private bool isMoveMode = true; // true: move mode, false: rotate mode
    
    private void Awake() {
        Instance = this;
    }
    
    void Update()
    {
        if (target == null) return;
        if (OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick)) {
            isMoveMode = !isMoveMode;
        }

        Vector2 thumbstickVec = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        if (isMoveMode) {
            Move(thumbstickVec);
        } else {
            Rotate(thumbstickVec);
        }
    }

    private void Move(Vector2 thumbstickVec) {
        target.position += new Vector3(thumbstickVec.x, 0, thumbstickVec.y) * positionSpeed * Time.deltaTime;
    }

    private void Rotate(Vector2 thumbstickVec) {
        target.Rotate(Vector3.up, thumbstickVec.x * rotationSpeed * Time.deltaTime);
    }

}
