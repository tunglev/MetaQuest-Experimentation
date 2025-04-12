using System;
using UnityEngine;

/// <summary>
/// Represents a configurable input parameter that can be automatically handled by a ConfigHandler.
/// </summary>
/// <typeparam name="T">The type of the value (e.g., float, int, string)</typeparam>
[Serializable]
public class ConfigInput<T>
{
    [SerializeField] private string _label;
    [SerializeField] private T _value;
    [SerializeField] private T _minValue; // Used for numerical types
    [SerializeField] private T _maxValue; // Used for numerical types

    // Optional event that gets triggered when the value changes
    public event Action<T> OnValueChanged;

    // Properties
    public string Label => _label;
    public T MinValue => _minValue;
    public T MaxValue => _maxValue;

    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    // Default constructor
    public ConfigInput()
    {
        _label = typeof(T).Name;
    }

    // Constructor with label and value
    public ConfigInput(string label, T value)
    {
        _label = label;
        _value = value;
    }

    // Constructor with label, value, and range
    public ConfigInput(string label, T value, T minValue, T maxValue)
    {
        _label = label;
        _value = value;
        _minValue = minValue;
        _maxValue = maxValue;
    }


    // For float specific helpers
    public static class FloatConfig
    {
        public static ConfigInput<float> Create(string label, float value, float min = float.MinValue, float max = float.MaxValue)
        {
            return new ConfigInput<float>(label, value, min, max);
        }
    }

    // For int specific helpers
    public static class IntConfig
    {
        public static ConfigInput<int> Create(string label, int value, int min = int.MinValue, int max = int.MaxValue)
        {
            return new ConfigInput<int>(label, value, min, max);
        }
    }

    // For string specific helpers
    public static class StringConfig
    {
        public static ConfigInput<string> Create(string label, string value)
        {
            return new ConfigInput<string>(label, value, null, null);
        }
    }

    // For bool specific helpers
    public static class BoolConfig
    {
        public static ConfigInput<bool> Create(string label, bool value)
        {
            return new ConfigInput<bool>(label, value, false, true);
        }
    }
}