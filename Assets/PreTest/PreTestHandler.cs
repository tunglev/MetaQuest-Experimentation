using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PreTestHandler : MonoBehaviour
{
    public static SessionConfig SessionConfig = new();
    public TextMeshProUGUI frontText;
    public Transform orbit;
    public float orbitSpeed;
    public OVRInput.Controller controller;
    private Transform controllerTransform;
    public RandomSpawner randomSpawner;

    [Header("Next round by clicking")]
    public TextMeshProUGUI instructionTMP;
    public GameObject instructionCanvas;
    public GameObject startRoundCanvas;
    public TextMeshProUGUI progressTMP;

    [Header("Configure Session")]
    public TMP_InputField input_audioNVisual;
    public TMP_InputField input_audioOnly;
    public TMP_InputField input_visualOnly;

    public void FinishSessionConfig() {
        SessionConfig = new SessionConfig();
        SessionConfig.roundCount.audio_n_visual = int.Parse(input_audioNVisual.text);
        SessionConfig.roundCount.audio_only = int.Parse(input_audioOnly.text);
        SessionConfig.roundCount.visual_only = int.Parse(input_visualOnly.text);
        Debug.Log(SessionConfig);
    }


    private void Start()
    {
        controllerTransform = controller == OVRInput.Controller.RTouch ?
            GameObject.Find("OVRCameraRig/TrackingSpace/RightHandAnchor").transform :
            GameObject.Find("OVRCameraRig/TrackingSpace/LeftHandAnchor").transform;
    }

    [ContextMenu("StartSession")]
    public void StartSession()
    {
        StartCoroutine(TestRoundsHandler());
    }

    private int index = 0;
    [ContextMenu("Next Round")]
    public void StartRound()
    {   
        index++;
        int audioNVisualIndex = SessionConfig.roundCount.audio_n_visual;
        int audioOnlyIndex = audioNVisualIndex + SessionConfig.roundCount.audio_only;
        int visualOnlyIndex = audioOnlyIndex + SessionConfig.roundCount.visual_only;

        StartRoundManual(1, audioNVisualIndex, () => randomSpawner.Spawn(true, true), endAction: () =>
        {
            instructionTMP.text = "Audio: YES\nVisual: NO\n \nPoint your LEFT hand and pull the trigger in the direction where you think the sphere is. The sphere is INVISBLE and emit SOUND. The sphere will be visible for 3 seconds AFTER you pull the trigger.";
            instructionCanvas.SetActive(true);
        });
        StartRoundManual(audioNVisualIndex + 1, audioOnlyIndex, () => randomSpawner.Spawn(false, true), endAction: () =>
        {
            instructionTMP.text = "Audio: NO\nVisual: YES\n \nPoint your LEFT hand and pull the trigger in the direction of the sphere. The sphere is VISIBLE but play NO SOUND.";
            instructionCanvas.SetActive(true);
        });
        StartRoundManual(audioOnlyIndex + 1, visualOnlyIndex, () => randomSpawner.Spawn(true, false), endAction: () => frontText.text = $"Done! Export to {DataCollector.ExportCSV()}");
    }
    private void StartRoundManual(int min, int max, Action mainAction, Action endAction)
    {
        if (min <= index && index <= max)
        {
            StartCoroutine(Helper());
            IEnumerator Helper()
            {
                mainAction();
                clicked = false;
                allowClick = true;
                yield return new WaitUntil(hasClicked);
                progressTMP.text = $"{index} / {SessionConfig.roundCount.total()}";
                if (min <= index && index < max) startRoundCanvas.SetActive(true);
                if (index == max) endAction();
            }

        }
    }

    private bool clicked = false;
    private bool allowClick = false;
    private bool hasClicked()
    {
        return clicked;
    }
    IEnumerator TestRoundsHandler()
    {
        frontText.text = "Pre-test session started";
        yield return new WaitForSeconds(2);
        frontText.text = "Point to the sphere. Audio and visual rounds starting soon...";
        yield return new WaitForSeconds(5);
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
            frontText.text = "Find the sphere";
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
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) // right hand trigger (left hand is PrimaryIndexTrigger)
        {
            fire();
        }
    }
    [ContextMenu("ExportCSV")]
    public void ExportCSV() {
        DataCollector.ExportCSV();
    }

    [ContextMenu("fire")]
    public void fire()
    {
        if (!allowClick) return;
        OVRInput.SetControllerVibration(0.1f, 0.1f, controller);
        var handToAudio = RandomSpawner.sources[0].transform.position - controllerTransform.position;
        float errorAngle = Vector3.Angle(controllerTransform.forward, handToAudio);
        float horErr = VectorAngleUtils.GetHorizontalAngleDifference(controllerTransform.forward, handToAudio);
        float verErr = VectorAngleUtils.GetVerticalAngleDifference(controllerTransform.forward, handToAudio);
        Debug.LogWarning("Hor: " + horErr);
        Debug.LogWarning("Ver: " + verErr);
        DataCollector.DataList[^1].End().SetErrorAngle(errorAngle).SetHorError(horErr).SetVerError(verErr); //^1 means last index
        clicked = true;
        allowClick = false;
        randomSpawner.EnableAudio(false);
        randomSpawner.EnableVisibility(true, 5f);
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
    //
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
