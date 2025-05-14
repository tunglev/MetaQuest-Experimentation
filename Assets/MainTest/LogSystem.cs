using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogSystem : MonoBehaviour
{
    public static LogSystem Instance { get; private set; }
    [SerializeField] TextMeshProUGUI logText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Log(string message)
    {
        logText.text += message + "\n";
    }
}
