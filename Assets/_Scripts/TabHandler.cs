using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TabHandler : MonoBehaviour
{
    [SerializeField] private Button[] tabs;
    [SerializeField] private GameObject[] contents;
    void Start()
    {
        foreach (var tab in tabs) {
            tab.onClick.AddListener(() => {
                foreach (var content in contents) {
                    content.SetActive(content.name == tab.gameObject.name);
                }
            });
        }
        tabs[0].onClick.Invoke();
    }

}


