using UnityEngine;



public class PassthroughController : MonoBehaviour

{
    private OVRPassthroughLayer _ovrPassthroughLayer;

    private void Awake() {
        _ovrPassthroughLayer = FindObjectOfType<OVRCameraRig>().GetComponent<OVRPassthroughLayer>();
    }

    private void Update() {
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger)) // right hand trigger (left hand is PrimaryIndexTrigger)
        {
            _ovrPassthroughLayer.enabled = !_ovrPassthroughLayer.enabled;
        }
    }
}