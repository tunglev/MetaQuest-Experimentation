#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;

public class ButtonDebugHelper : MonoBehaviour
{
    [ContextMenu("Execute OnClick")]
    public void ExecuteOnClick() {
        GetComponent<Button>().onClick.Invoke();
    }
}
#endif