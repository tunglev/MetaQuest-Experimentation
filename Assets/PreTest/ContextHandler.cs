using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ContextHandler : MonoBehaviour
{
    public Transform orbit;
    public float orbitSpeed;
    public OVRInput.Controller controller;
    private Transform controllerTransform;
    private void Start()
    {
        Next();
        controllerTransform = controller == OVRInput.Controller.RTouch ?
            GameObject.Find("OVRCameraRig/TrackingSpace/RightHandAnchor").transform :
            GameObject.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").transform;
    }
    private void Update()
    {
        orbit.transform.Rotate(orbit.transform.up, Time.deltaTime * orbitSpeed);
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //Next();
            
        }
        //ShowRenderers(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger));
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            fire();
        }
    }

    [ContextMenu("ExportCSV")]
    public void ExportCSV()
    {
        var sb = new StringBuilder(Data.COLUMNS);
        for (int i=0; i<5;i++)
        {
            var content = sb.Append('\n').Append(new Data().SetErrorAngle(34).ToString()).ToString();
            var folder = Application.persistentDataPath;
            var filePath = Path.Combine(folder, $"{DateTime.Now:y-M-d HH_mm_ss} export.csv");
            File.WriteAllText(filePath, content);
            AssetDatabase.Refresh();
            Debug.Log($"CSV file written to \"{filePath}\"");
        }
    }

    private Data data;
    [ContextMenu("fire")]
    public void fire()
    {
        print(data);
        data = new Data().IsVisible(true).SetErrorAngle(29);
        OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.LTouch);
        var handToAudio = RandomSpawner.sources[0].transform.position - controllerTransform.position;
        Debug.LogWarning(Vector3.Angle(controllerTransform.forward, handToAudio));
    }

    private bool isVisible = false;
    public void ShowRenderers(bool value)
    {
        if (value == isVisible) return;
        var renderers = FindObjectsOfType<Renderer>();
        foreach(Renderer renderer in renderers)
        {
            renderer.enabled = value;
        }
        isVisible = value;
    }
    public GameObject[] scenarios;
    private int i = 0;
    [ContextMenu("Next")]
    public void Next()
    {
        SelectScenario(i++ % scenarios.Length);
    }
    public void SelectScenario(int index)
    {
        foreach (var scenario in scenarios)
        {
            scenario.SetActive(false);
        }
        scenarios[index].SetActive(true);
    }

    public void SetAudioClip(AudioClip clip)
    {
        var sources = FindObjectsOfType<AudioSource>(includeInactive: true);
        foreach(var source in sources)
        {
            source.clip = clip;
            source.Play();
        }
        FindObjectOfType<RandomSpawner>().curClip = clip;
    }
}
