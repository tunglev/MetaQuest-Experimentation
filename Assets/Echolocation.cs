using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Echolocation : MonoBehaviour
{
    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var walls = MRUK.Instance.GetCurrentRoom().WallAnchors;
        foreach (var wall in walls)
        {

        }
    }
}
