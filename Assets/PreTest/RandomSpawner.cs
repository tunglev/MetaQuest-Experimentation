using System;
using System.Collections;
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
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        while (gameObject.activeInHierarchy)
        {
            Spawn();
            yield return new WaitForSeconds(spawnEverySec);

        }
    }
    

    public void Spawn()
    {
        Vector3 pos = _cam.transform.position + new Vector3(Random.Range(xRange.min, xRange.max), 0, Random.Range(zRange.min, zRange.max));
        var temp = Instantiate(prefab, pos, Quaternion.identity);
        Destroy(temp.gameObject, 2);
    }
}
