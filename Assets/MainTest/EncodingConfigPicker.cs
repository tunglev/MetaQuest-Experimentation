using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using TMPro;

public class EncodingConfigPicker : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputFieldPrefab;
    [SerializeField]
    private GameObject container;
    private EncodingMethod target;

    public void SetTarget(EncodingMethod method)
    {
        if (method == null)
        {
            Debug.LogError("Target EncodingMethod is null.");
            return;
        }
        // get the ConfigInput fields of the encoding method
        target = method;
        
        // Get ConfigInput fields and process them
        var configInputFields = GetConfigInputFields(target);

        ClearContainerChildren();
        
        // You can use the fields for whatever you need
        foreach (var field in configInputFields)
        {
            Debug.Log($"Found ConfigInput field: {field.Name}");
            // spawn input field prefab
            var inputFieldUI = Instantiate(inputFieldPrefab, container.transform);
            
            // Get the ConfigInput instance
            var configInputInstance = field.GetValue(target);
            
            // Get the generic type argument (T in ConfigInput<T>)
            Type genericType = field.FieldType.GenericTypeArguments[0];
            
            // Cast and manipulate using the generic helper method
            if (genericType == typeof(float))
            {
                var typedField = GetTypedConfigInput<float>(configInputInstance);
                InitInputFieldUI(inputFieldUI, typedField);
            }
            else if (genericType == typeof(int))
            {
                var typedField = GetTypedConfigInput<int>(configInputInstance);
                InitInputFieldUI(inputFieldUI, typedField);
            }
            else if (genericType == typeof(bool))
            {
                var typedField = GetTypedConfigInput<bool>(configInputInstance);
                InitInputFieldUI(inputFieldUI, typedField);
            }
            else if (genericType == typeof(string))
            {
                var typedField = GetTypedConfigInput<string>(configInputInstance);
                InitInputFieldUI(inputFieldUI, typedField);
            }
            // Add more type cases as needed
            
            // Here you can create UI elements in the container for each field
            // or do whatever else you need with these fields
        }
    }

    // T can only be int, float, string, or bool
    
    public void InitInputFieldUI<T>(TMP_InputField inputField, ConfigInput<T> configInput)
    {
        // Initialize the input field UI with the config input properties
        inputField.text = configInput.Value.ToString();
        inputField.transform.Find("label").GetComponent<TextMeshProUGUI>().text = configInput.Label;

        // check the type of T and set the contenttype accordingly
        if (configInput is ConfigInput<int> intConfigInput){
            inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            // Add listeners to handle value changes
            inputField.onValueChanged.AddListener((value) =>
            {
                if (int.TryParse(value, out int newValue))
                {
                    intConfigInput.Value = newValue;
                }
                else
                {
                    Debug.LogError($"Invalid input for int: {value}");
                }
            });
        }
        else if (configInput is ConfigInput<float> floatConfigInput) {
            inputField.contentType = TMP_InputField.ContentType.DecimalNumber;
            // Add listeners to handle value changes
            inputField.onValueChanged.AddListener((value) =>
            {
                if (float.TryParse(value, out float newValue))
                {
                    floatConfigInput.Value = newValue;
                }
                else
                {
                    Debug.LogError($"Invalid input for float: {value}");
                }
            });
        }
        else if (configInput is ConfigInput<string> stringConfigInput) {
            inputField.contentType = TMP_InputField.ContentType.Standard;
            // Add listeners to handle value changes
            inputField.onValueChanged.AddListener((value) =>
            {
                stringConfigInput.Value = value;
            });
        }
        else if (configInput is ConfigInput<bool> boolConfigInput)  // Handle boolean type if needed
        {
            inputField.contentType = TMP_InputField.ContentType.Standard; // or whatever is appropriate
            // Add listeners to handle value changes
            inputField.onValueChanged.AddListener((value) =>
            {
                boolConfigInput.Value = bool.Parse(value);
            });
        }
    }
    
    // Helper method to cast an object to ConfigInput<T>
    private ConfigInput<T> GetTypedConfigInput<T>(object configInputInstance)
    {
        return (ConfigInput<T>)configInputInstance;
    }
    
    /// <summary>
    /// Gets all fields of type ConfigInput from the given object
    /// </summary>
    /// <param name="obj">The object to inspect</param>
    /// <returns>List of FieldInfo for fields that are ConfigInput types</returns>
    private List<FieldInfo> GetConfigInputFields(object obj)
    {
        List<FieldInfo> result = new List<FieldInfo>();
        
        if (obj == null) return result;
        
        Type type = obj.GetType();
        
        // Get all fields (public, private, protected, etc.)
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                                      
        
        foreach (var field in fields)
        {
            Type fieldType = field.FieldType;
            
            // Check if the field type is ConfigInput or derives from ConfigInput
            if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(ConfigInput<>))
            {
                result.Add(field);
            }
        }
        
        return result;
    }
    [ContextMenu("Test")]
    void Test()
    {
        SetTarget(target);
    }

    private void ClearContainerChildren() {
        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
