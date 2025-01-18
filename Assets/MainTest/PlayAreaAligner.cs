using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OVRCameraRig))]
public class PlayAreaAligner : MonoBehaviour
{
    public Vector3 GetPlayAreaDimensions() {
        // NOTE: require OVRCameraRig to be able to access OVRManager.boundary
        return OVRManager.boundary.GetDimensions(OVRBoundary.BoundaryType.PlayArea); 
    }
}
