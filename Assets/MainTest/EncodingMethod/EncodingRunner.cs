using UnityEngine;

public struct EncodingMethodPair {
    public EncodingMethod globalEncoding;
    public EncodingMethod specializedEncoding;
}
public class EncodingRunner : MonoBehaviour
{
    [SerializeField]
    private EncodingMethod[] _globalEncodingArray;
    [SerializeField]
    private EncodingMethod[] _specializedEncodingArray;
    private EncodingMethodPair _currentEncodingPair;
    private GameObject _centerEye;

    private void Awake() {
        _centerEye = Camera.main.gameObject;
    }

    private void Start() {
#if UNITY_EDITOR
        UpdateEncodingPair(0, 'g'); // select 0 (none) for global
        UpdateEncodingPair(0, 's'); // select 0 (none) for specialized
#else
        UpdateEncodingPair(1, 'g'); // select 1 for global
        UpdateEncodingPair(1, 's'); // select 1 for specialized
#endif
        MainTestHandler.Instance.OnEncodingChanged += UpdateEncodingPair;
    }

    private void UpdateEncodingPair(int encodingIndex, char encodingType) {
        if (encodingType == 'g') { // global
            EncodingMethod newEncoding = _globalEncodingArray[encodingIndex];
            InitEncodingMethod(newEncoding);
            _currentEncodingPair.globalEncoding = newEncoding;
        } else if (encodingType == 's') { // specialized
            EncodingMethod newEncoding = _specializedEncodingArray[encodingIndex];
            InitEncodingMethod(newEncoding);
            _currentEncodingPair.specializedEncoding = newEncoding;
        }
    }

    private void InitEncodingMethod(EncodingMethod encoding) {
        if (encoding == null) return;
        encoding.InitOnCam(_centerEye);
        encoding.enabled = true;
        encoding.IsInit = true;
    }

    private void Update() {
        if (_currentEncodingPair.specializedEncoding != null) {
            if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger)) // right hand trigger (left hand is PrimaryIndexTrigger)
            {
                TriggerDownSpecialized();
            }
            if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
            {
                TriggerUpSpecialized();
            }
        }
        if (_currentEncodingPair.globalEncoding != null) {
            if (OVRInput.GetDown(OVRInput.RawButton.B)) // 
            {
                TriggerDownGlobal();
            }
            if (OVRInput.GetUp(OVRInput.RawButton.B))
            {
                TriggerUpGlobal();
            }
        }
            
    }

    public void TriggerDownGlobal() {
        _currentEncodingPair.globalEncoding.OnDemandTriggeredDown();
        OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.RTouch);
    }
    public void TriggerUpGlobal() {
        _currentEncodingPair.globalEncoding.OnDemandTriggeredUp();
    }
    public void TriggerDownSpecialized() {
        _currentEncodingPair.specializedEncoding.OnDemandTriggeredDown();
    }
    public void TriggerUpSpecialized() {
        _currentEncodingPair.specializedEncoding.OnDemandTriggeredUp();
    }
}
