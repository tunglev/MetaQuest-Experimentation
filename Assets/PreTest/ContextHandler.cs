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
        ShowRenderers(OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger));
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
