using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public struct TestSubjectSessionConfig
{
    public int orientedSessionLoopCount;
    public int sonificationSessionLoopCount;
    public int blindSessionLoopCount;
}
public class TestSubjectHandler : MonoBehaviour
{
    public static TestSubjectHandler Instance { get; private set; }
    private void Awake()
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
    
    [SerializeField] TMP_InputField orientedSessionLoopCountInput;
    [SerializeField] TMP_InputField sonificationSessionLoopCountInput;
    [SerializeField] TMP_InputField blindSessionLoopCountInput;
    [SerializeField] Button startTestSubjectSessionButton;
    [SerializeField] GameObject nextPanel;
    private EncodingRunner _encodingRunner;
    private TestSubjectSessionConfig _currentConfig;
    private void Start()
    {
        _encodingRunner = FindObjectOfType<EncodingRunner>();
        _currentConfig = new TestSubjectSessionConfig
        {
            orientedSessionLoopCount = 3,
            sonificationSessionLoopCount = 3,
            blindSessionLoopCount = 3
        };
        orientedSessionLoopCountInput.text = _currentConfig.orientedSessionLoopCount.ToString();
        sonificationSessionLoopCountInput.text = _currentConfig.sonificationSessionLoopCount.ToString();
        blindSessionLoopCountInput.text = _currentConfig.blindSessionLoopCount.ToString();

        orientedSessionLoopCountInput.onValueChanged.AddListener(str =>
        {
            if (int.TryParse(str, out int value))
            {
                _currentConfig.orientedSessionLoopCount = value;
            }
        });
        sonificationSessionLoopCountInput.onValueChanged.AddListener(str =>
        {
            if (int.TryParse(str, out int value))
            {
                _currentConfig.sonificationSessionLoopCount = value;
            }
        });
        blindSessionLoopCountInput.onValueChanged.AddListener(str =>
        {
            if (int.TryParse(str, out int value))
            {
                _currentConfig.blindSessionLoopCount = value;
            }
        });
        startTestSubjectSessionButton.onClick.AddListener(StartTestSubjectSession);
        nextPanel.GetComponentInChildren<Button>().onClick.AddListener(Next);
        nextPanel.SetActive(false);
    }

    private void StartTestSubjectSession() {
        if (EncodingRunner._currentEncodingPair.globalEncoding == null && EncodingRunner._currentEncodingPair.specializedEncoding == null) {
            LogSystem.Instance.Log("Cannot start tester session. No encoding method selected");
            return;
        }
        StartCoroutine(StartTestSubjectSessionCoroutine(_currentConfig));
    }
    private int i = 0;
    private string currentSessionName = "Admin Session";
    public IEnumerator StartTestSubjectSessionCoroutine(TestSubjectSessionConfig config)
    {
        i=0;
        MainTestHandler.Instance.AdminMode = false;
        nextPanel.SetActive(false);
        _encodingRunner.IsOn = false;
        MainTestHandler.Instance.SpawnNewRoom();
        currentSessionName = "Oriented Session";
        yield return new WaitWhile(() => i < config.orientedSessionLoopCount);
        LogSystem.Instance.Log(i+ " Oriented Session Loop completed");

        i = 0; 
        _encodingRunner.IsOn = true;
        currentSessionName = "Sonification Session";
        yield return new WaitWhile(() => i < config.sonificationSessionLoopCount);
        LogSystem.Instance.Log(i+ " Sonification Session Loop completed: ");

        i=0;
        MainTestHandler.Instance.ToggleBlindMode();
        currentSessionName = "Blind Session";
        yield return new WaitWhile(() => i < config.blindSessionLoopCount);
        LogSystem.Instance.Log(i+" Blind Session Loop completed: ");
        i = 0;
        currentSessionName = "Admin Session";
        MainTestHandler.Instance.AdminMode = true;
    }

    private void Next() {
        i++;
        Debug.Log(i);
        MainTestHandler.Instance.SpawnNewRoom();
        nextPanel.SetActive(false);
    }
    public void SpawnNextPanel() {
        nextPanel.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.2f;
        nextPanel.transform.forward = Camera.main.transform.forward;
        nextPanel.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = $"{currentSessionName}\n {i+1}\n Completed";
        nextPanel.SetActive(true);
    }
}
