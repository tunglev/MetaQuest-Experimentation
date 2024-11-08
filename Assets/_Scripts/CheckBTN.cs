using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CheckBTN : MonoBehaviour
{
    public UnityEvent onChecked;
    public UnityEvent onUnchecked;

    private bool isChecked = false;
    private GameObject checkMark;

    private void Awake()
    {
        checkMark = transform.GetChild(0).gameObject;
        checkMark.SetActive(isChecked);
        onUnchecked?.Invoke();
        GetComponent<Button>().onClick.AddListener(Toggle); 
    }

    [ContextMenu("TestToggle")]
    private void Toggle()
    {
        isChecked = !isChecked;
        checkMark.SetActive(isChecked);
        if (isChecked) onChecked?.Invoke();
        else onUnchecked?.Invoke();
    }
}
