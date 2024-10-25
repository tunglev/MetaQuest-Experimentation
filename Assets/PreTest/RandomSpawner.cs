using Meta.XR.MRUtilityKit;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpawner : MonoBehaviour
{
    public AudioSource prefab;
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
        
    }



    public AudioClip curClip;
    public static List<AudioSource> sources = new();

    private SphericalCoord TwoD()
    {
        return new SphericalCoord(r: Random.Range(0.2f, 3), theta: 0f, phi: Random.Range(0f, 2f * Mathf.PI));
    }
    private SphericalCoord ThreeD()
    {
        return new SphericalCoord(r: Random.Range(0.2f, 3), theta: Random.Range(0f, Mathf.PI), phi: Random.Range(0f, Mathf.PI * 2));
    }

    [ContextMenu("SpawnTwoD")]
    public void SpawnTwoD()
    {
        //XZ plane
        Spawn(TwoD, count: 1);//
    }
    [ContextMenu("SpawnTwoD_Multi")]
    public void SpawnTwoD_Multi()
    {
        Spawn(TwoD, count: 2);//
    }

    [ContextMenu("SpawnThreeD")]
    public void SpawnThreeD()
    {
        Spawn(ThreeD , count:1);
    }
    [ContextMenu("SpawnThreeD_Multi")]
    public void SpawnThreeD_Multi()
    {
        Spawn(ThreeD, count: 2);
    }

    [ContextMenu("Spawn5")]
    public void Spawn5()
    {
        Spawn(ThreeD, count:5);
    }
    private void Spawn(Func<SphericalCoord> getSpherePos ,int count = 5)
    {
        DestroyAllSrcs();   
        for (int i = 1; i <= count; i++)
        {
            var pos = getSpherePos();
            var src = Instantiate(prefab, _cam.transform.position + pos.ToCartesian(), Quaternion.identity, transform);
            DataCollector.DataList.Add(new Data().SetPos(pos).SetAudioFileName(curClip.name).Start());
            src.clip = curClip;
            src.Play();
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
