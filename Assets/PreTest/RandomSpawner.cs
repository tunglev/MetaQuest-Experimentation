using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpawner : MonoBehaviour
{
    public AudioSource prefab;
    public float spawnEverySec;
    [Serializable]
    public struct Range {
        public float min, max;
    }
    
    public Range xRange;
    public Range yRange;
    public Range zRange;

    private Camera _cam;
    private void Start()
    {
        _cam = Camera.main;
        /*StartCoroutine(Spawner());*/
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            Spawn();
        }
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            SpawnMultiple();
        }
    }
    IEnumerator Spawner()
    {
        while (gameObject.activeInHierarchy)
        {
            Spawn();
            yield return new WaitForSeconds(spawnEverySec);

        }
    }

    public AudioClip curClip;
    private AudioSource curAudio;
    [ContextMenu("Spawn")]
    public void Spawn()
    {
        DestroyAllSrcs();
        Vector3 pos = _cam.transform.position + new Vector3(Random.Range(xRange.min, xRange.max), 0, Random.Range(zRange.min, zRange.max));
        curAudio = Instantiate(prefab, pos, Quaternion.identity, transform);
        curAudio.clip = curClip;//
    }

    private List<AudioSource> sources = new();
    const float COUNT = 2;
    public void SpawnMultiple()
    {
        DestroyAllSrcs();   
        for (int i = 1; i <= COUNT; i++)
        {
            Vector3 pos = _cam.transform.position + new Vector3(Random.Range(xRange.min, xRange.max), 0, Random.Range(zRange.min, zRange.max));
            var src = Instantiate(prefab, pos, Quaternion.identity, transform);
            src.clip = curClip;
            sources.Add(src);
        }
    }

    private void DestroyAllSrcs()
    {
        if (curAudio) Destroy(curAudio.gameObject);
        foreach (var source in sources)
        {
            sources.Remove(source);
            Destroy(source.gameObject);
        }
    }
}
