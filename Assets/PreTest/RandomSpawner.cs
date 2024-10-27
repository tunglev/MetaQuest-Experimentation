using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpawner : MonoBehaviour
{
    public AudioSource prefab;
    public static List<AudioSource> sources = new();

    private SphericalCoord TwoD()
    {
        var radius = PreTestHandler.SessionConfig.radiusRange;
        return new SphericalCoord(r: Random.Range(radius.x, radius.y), theta: 0f, phi: Random.Range(0f, 2f * Mathf.PI));
    }
    private SphericalCoord ThreeD()
    {
        var radius = PreTestHandler.SessionConfig.radiusRange;
        return new SphericalCoord(r: Random.Range(radius.x, radius.y), theta: Random.Range(0f, Mathf.PI), phi: Random.Range(0f, Mathf.PI * 2));
    }
    public void Spawn()
    {
        if (PreTestHandler.SessionConfig.threeD)
        {
            Spawn(ThreeD, count: 1);
        }
        else
        {
            Spawn(TwoD, count: 1);
        }
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
            var src = Instantiate(prefab, Camera.main.transform.position + pos.ToCartesian(), Quaternion.identity, transform);
            DataCollector.DataList.Add(new Data().SetPos(pos).SetAudioFileName(PreTestHandler.SessionConfig.audioFile.name).Start());
            src.clip = PreTestHandler.SessionConfig.audioFile;
            print(PreTestHandler.SessionConfig.audioFile.name);
            src.Play();
            sources.Add(src);
        }
    }

    public void DestroyAllSrcs()
    {
        foreach (var source in sources)
        {
            Destroy(source.gameObject);
        }
        sources.Clear();
    }

    public void EnableVisibility(bool val)
    {
        sources.ForEach(e => e.GetComponent<Renderer>().enabled = val);
    }

    public void EnableAudio(bool val)
    {
        sources.ForEach(e => {
            if (val) e.Play();
            else e.Stop();
        });
    }
}
