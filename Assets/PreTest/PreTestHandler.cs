using System.Collections;
using TMPro;
using UnityEngine;

public class PreTestHandler : MonoBehaviour
{
    public static SessionConfig SessionConfig;
    public SessionConfig initConfig;
    public TextMeshProUGUI frontText;
    public Transform orbit;
    public float orbitSpeed;
    public OVRInput.Controller controller;
    private Transform controllerTransform;
    public RandomSpawner randomSpawner;
    private void Start()
    {
        SessionConfig = initConfig;
        controllerTransform = controller == OVRInput.Controller.RTouch ?
            GameObject.Find("OVRCameraRig/TrackingSpace/RightHandAnchor").transform :
            GameObject.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").transform;

        StartCoroutine(TestRoundsHandler());
    }

    private bool clicked = false;
    private bool allowClick = false;
    private bool hasClicked()
    {
        return clicked;
    }
    IEnumerator TestRoundsHandler()
    {
        for (int i = 1; i<= SessionConfig.roundCount.audio_n_visual; i++)
        {
            frontText.text = "";
            randomSpawner.Spawn(hasVisual: true, hasAudio: true);
            clicked = false;
            allowClick = true;
            yield return new WaitUntil(hasClicked);
            allowClick = false;
            randomSpawner.EnableAudio(false);
            frontText.text = $"Data registered {i} / {SessionConfig.roundCount.audio_n_visual}. Spawning new audio source...";
            yield return new WaitForSeconds(2);
        }

        frontText.text = "Audio Only Rounds Starting soon";
        yield return new WaitForSeconds(3);
        for (int i = 1; i <= SessionConfig.roundCount.audio_only; i++)
        {
            frontText.text = "";
            randomSpawner.Spawn(hasVisual: false, hasAudio: true);
            clicked = false; allowClick = true;
            yield return new WaitUntil(hasClicked);
            allowClick = false;
            randomSpawner.EnableAudio(false);
            frontText.text = $"Data registered {i} / {SessionConfig.roundCount.audio_only}. Spawning new audio source...";
            yield return new WaitForSeconds(2);
        }

        frontText.text = "Visual Only Rounds Starting soon";
        yield return new WaitForSeconds(3);
        for (int i = 1; i <= SessionConfig.roundCount.visual_only; i++)
        {
            frontText.text = "";
            randomSpawner.Spawn(hasVisual: true, hasAudio: false);
            clicked = false; allowClick = true;
            yield return new WaitUntil(hasClicked);
            allowClick = false;
            randomSpawner.EnableAudio(false);
            frontText.text = $"Data registered {i} / {SessionConfig.roundCount.visual_only}. Spawning new audio source...";
            yield return new WaitForSeconds(2);
        }

        frontText.text = $"Done! Export to {DataCollector.ExportCSV()}";
    }
    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            fire();
        }
    }

    [ContextMenu("ExportCSV")]
    public void ExportCSV()
    {
        DataCollector.ExportCSV();//
    }

    [ContextMenu("fire")]
    public void fire()
    {
        if (!allowClick) return;
        OVRInput.SetControllerVibration(0.1f, 0.1f, OVRInput.Controller.LTouch);
        var handToAudio = RandomSpawner.sources[0].transform.position - controllerTransform.position;
        float errorAngle = Vector3.Angle(controllerTransform.forward, handToAudio);
        Debug.LogWarning(errorAngle);
        DataCollector.DataList[^1].End().SetErrorAngle(errorAngle); //^1 means last index
        clicked = true;
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
        SessionConfig.audioFile = clip;
        /*var sources = FindObjectsOfType<AudioSource>(includeInactive: true);
        foreach(var source in sources)
        {
            source.clip = clip;
            source.Play();
        }
        FindObjectOfType<RandomSpawner>().curClip = clip;*/
    }
}
