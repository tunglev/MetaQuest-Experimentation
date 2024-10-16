using UnityEngine;

public class ContextHandler : MonoBehaviour
{
    public Transform orbit;
    public float orbitSpeed;
    private void Start()
    {
        Next();
    }
    private void Update()
    {
        orbit.transform.Rotate(orbit.transform.up, Time.deltaTime * orbitSpeed);
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            Next();
        }
    }
    public GameObject[] contexts;
    private int i = 0;
    [ContextMenu("Next")]
    public void Next()
    {
        foreach (var context in contexts)
        {
            context.SetActive(false);
        }
        contexts[i++ % contexts.Length].SetActive(true);
    }
}
