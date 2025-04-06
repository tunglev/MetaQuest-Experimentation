using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualStick : EncodingMethod
{
    [SerializeField]
    private AudioSource audioSrcPrefab;
    private AudioSource audioSrc;

    private Transform rightController;
    private bool isProjecting = false;

    public override void OnDemandTriggeredDown()
    {
        StartRayProjection();
    }

    public override void OnDemandTriggeredUp()
    {
        StopRayProjection();
    }

    public override void InitOnCam(GameObject centerEye)
    {
        rightController = FindObjectOfType<OVRCameraRig>().rightControllerAnchor;
    }

    private void StartRayProjection()
    {
        isProjecting = true;
    }

    private void StopRayProjection()
    {
        isProjecting = false;
    }

    void Update()
    {
        if (isProjecting)
        {
            if (Physics.Raycast(rightController.position, rightController.forward, out RaycastHit hit))
            {
                if (audioSrc == null)
                {
                    audioSrc = Instantiate(audioSrcPrefab, hit.point, Quaternion.identity);
                }

                float distance = (hit.point - rightController.position).magnitude;
                audioSrc.transform.position = hit.point;
                //audioSrc.pitch = Mathf.Lerp(3f, 0f, distance / 10f);
                if (!audioSrc.isPlaying)
                {
                    audioSrc.Play();
                }
            }
            else
            {
                StopAudio();
            }
        }
        else
        {
            StopAudio();
        }
    }

    private void StopAudio()
    {
        if (audioSrc != null && audioSrc.isPlaying)
        {
            print("stopping");
            audioSrc.Stop();
        }
    }
}
