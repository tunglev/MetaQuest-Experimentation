using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(ButtonDebugHelper))]
public class ButtonDebugHelperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector fields
        DrawDefaultInspector();

        // Get the target script instance
        ButtonDebugHelper script = (ButtonDebugHelper)target;

        // Add a button to the inspector
        if (GUILayout.Button("OnClick"))
        {
            // Call the public method on the target script
            script.ExecuteOnClick();
        }
    }
}
