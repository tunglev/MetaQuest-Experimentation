using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControllerPanel : MonoBehaviour
{
    [Header("Settings")]
    public Button spawnRoomButton;
    public Button blindModeButton;
    public Toggle toogleUseFixRoomSize;

    [Header("Encoding")]
    public TMP_Dropdown globalEncodingDropdown;
    public TMP_Dropdown specializedEncodingDropdown;

    [Header("Other")]
    public GameObject aimLine;

    void OnEnable()
    {
        aimLine.SetActive(true);
    }

    void OnDisable()
    {
        if (aimLine != null)
        {
            aimLine.SetActive(false);
        }
    }
}
