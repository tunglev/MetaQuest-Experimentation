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
    [SerializeField] EncodingConfigPicker g_encodingConfigPicker;
    [SerializeField] EncodingConfigPicker s_encodingConfigPicker;
    [SerializeField] private Button g_encodingConfigButton;
    [SerializeField] private Button s_encodingConfigButton;

    [Header("Other")]
    public GameObject aimLine;

    void Awake()
    {
        g_encodingConfigButton.onClick.AddListener(() =>
        {
            bool toggledVal = !g_encodingConfigPicker.gameObject.activeSelf;
            g_encodingConfigPicker.gameObject.SetActive(toggledVal);
            if (toggledVal) g_encodingConfigPicker.SetTarget(EncodingRunner._currentEncodingPair.globalEncoding);
        });
        s_encodingConfigButton.onClick.AddListener(() =>
        {
            bool toggledVal = !s_encodingConfigPicker.gameObject.activeSelf;
            s_encodingConfigPicker.gameObject.SetActive(toggledVal);
            if (toggledVal) s_encodingConfigPicker.SetTarget(EncodingRunner._currentEncodingPair.specializedEncoding);});
    }
    void OnEnable()
    {
        aimLine.SetActive(true);
        g_encodingConfigPicker.gameObject.SetActive(false);
        s_encodingConfigPicker.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (aimLine != null)
        {
            aimLine.SetActive(false);
        }
    }
}
