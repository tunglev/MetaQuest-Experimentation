using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EncodingMethod : MonoBehaviour
{
    [HideInInspector] public bool IsInit = false;
    public virtual void InitOnCam(GameObject centerEye) {
    }
    // trigger on demand
    public virtual void OnDemandTriggeredDown() {}
    public virtual void OnDemandTriggeredUp() { }
}
