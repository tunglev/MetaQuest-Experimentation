using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class TMPInputFieldDebugHelper : MonoBehaviour
{
    private TMP_InputField targetInputField;

    void Awake()
    {
        targetInputField = GetComponent<TMP_InputField>();
    }

    // This method will be called by the editor script
    public void SetValueAndNotify(string newValue)
    {
        if (targetInputField != null)
        {
            targetInputField.text = newValue;
            // Setting the text property usually triggers onValueChanged automatically.
            // If not, uncomment the line below:
            // targetInputField.onValueChanged.Invoke(newValue);
        }
        else
        {
            Debug.LogWarning("Target TMP_InputField is not assigned.", this);
        }
    }
}
