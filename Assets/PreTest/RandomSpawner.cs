using Meta.XR.MRUtilityKit;
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
            Spawn5();
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
    public static List<AudioSource> sources = new();
    [ContextMenu("SpawnXZ")]
    public void SpawnXZ()
    {
        SpawnMultiple(() => new SphericalCoord(r: Random.Range(0.2f, 3), theta: 0f, phi: Random.Range(0f, 2f * Mathf.PI)), count: 10);//
    }
    [ContextMenu("SpawnSingle")]
    public void Spawn()
    {
        SpawnMultiple(()=> new SphericalCoord(r: Random.Range(0.2f, 3), theta: Random.Range(0f, Mathf.PI), phi: Random.Range(0f, Mathf.PI * 2)), count:1);
    }

    [ContextMenu("SpawnMultiple")]
    public void Spawn5()
    {
        SpawnMultiple(()=> new SphericalCoord(r: Random.Range(0.2f, 3), theta: Random.Range(0f, Mathf.PI), phi: Random.Range(0f, Mathf.PI * 2)), count:5);
    }
    public void SpawnMultiple(Func<SphericalCoord> getSpherePos ,int count = 5)
    {
        DestroyAllSrcs();   
        for (int i = 1; i <= count; i++)
        {
            var pos = getSpherePos();
            //SphericalCoord pos = new SphericalCoord(r: Random.Range(0.2f, 3), theta: Random.Range(0f, Mathf.PI), phi: Random.Range(0f, Mathf.PI * 2));
            print(pos);
            var src = Instantiate(prefab, _cam.transform.position + pos.ToCartesian(), Quaternion.identity, transform);
            src.clip = curClip;
            sources.Add(src);
        }
    }

    private void DestroyAllSrcs()
    {
        foreach (var source in sources)
        {
            Destroy(source.gameObject);
        }
        sources.Clear();
    }
}
