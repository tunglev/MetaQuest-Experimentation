using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(TMPInputFieldDebugHelper))]
public class TMPInputFieldDebugHelperEditor : Editor
{
    private string _valueToSet = ""; // Store the value entered in the inspector

    public override void OnInspectorGUI()
    {
        // Draw the default inspector fields (for the targetInputField reference)
        DrawDefaultInspector();

        // Get the target script instance
        TMPInputFieldDebugHelper script = (TMPInputFieldDebugHelper)target;

        

        EditorGUILayout.Space(); // Add some spacing
        EditorGUILayout.LabelField("Debug Controls", EditorStyles.boldLabel);

        // Add a text field to input the desired value
        _valueToSet = EditorGUILayout.TextField("Value", _valueToSet);

        // Add a button to the inspector
        if (GUILayout.Button("Set Value & Trigger OnChange"))
        {
            // Call the public method on the target script
            script.SetValueAndNotify(_valueToSet);
        }
    }
}