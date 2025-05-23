using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
  

    [ContextMenu("SpawnTwoD")]
    public void SpawnTwoD()
    {
        //XZ plane
        Spawn(TwoD, count: 1, true, true);//
    }
    [ContextMenu("SpawnTwoD_Multi")]
    public void SpawnTwoD_Multi()
    {
        Spawn(TwoD, count: 2, true, true);//
    }

    [ContextMenu("SpawnThreeD")]
    public void SpawnThreeD()
    {
        Spawn(ThreeD, count: 1, true, true);
    }
    [ContextMenu("SpawnThreeD_Multi")]
    public void SpawnThreeD_Multi()
    {
        Spawn(ThreeD, count: 2, true, true);
    }

    [ContextMenu("Spawn5")]
    public void Spawn5()
    {
        Spawn(ThreeD, count:5, true, true);
    }

    public void Spawn(bool hasVisual, bool hasAudio)
    {
        if (PreTestHandler.SessionConfig.threeD)
        {
            Spawn(ThreeD, count: 1, hasVisual, hasAudio);
        }
        else
        {
            Spawn(TwoD, count: 1, hasVisual, hasAudio);
        }
    }

    private void Spawn(Func<SphericalCoord> getSpherePos ,int count, bool hasVisual, bool hasAudio)
    {
        DestroyAllSrcs();   
        for (int i = 1; i <= count; i++)
        {
            var pos = getSpherePos();
            var src = Instantiate(prefab, Camera.main.transform.position + pos.ToCartesian(), Quaternion.identity, transform);
            DataCollector.DataList.Add(new Data().IsVisible(hasVisual).HasAudio(hasAudio).SetPos(pos).SetAudioFileName(PreTestHandler.SessionConfig.audioFile.name).Start());
            src.clip = PreTestHandler.SessionConfig.audioFile;
            if (hasAudio) src.Play();
            if (!hasVisual) src.gameObject.GetComponentInChildren<Renderer>().enabled = false;
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

    public void EnableVisibility(bool val, float duration = -1)
    {
        if (duration == -1)
        {
            sources.ForEach(e => e.GetComponentInChildren<Renderer>().enabled = val);
        } 
        else
        {
            StartCoroutine(temp());
        }
        IEnumerator temp()
        {
            List<Renderer> components = sources.Select(e => e.GetComponentInChildren<Renderer>()).ToList();
            List<bool> originalVal = components.Select(e => e.enabled).ToList();
            components.ForEach(e => e.enabled = val);
            yield return new WaitForSeconds(duration);
            for (int i = 0; i< components.Count; i++)
            {
                components[i].enabled = originalVal[i];
            }
        }
    }

    public void EnableAudio(bool val)
    {
        sources.ForEach(e => {
            if (val) e.Play();
            else e.Stop();
        });
    }
}
