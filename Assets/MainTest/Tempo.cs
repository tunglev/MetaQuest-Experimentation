using System;
using UnityEngine;

public class TriggerEverySeconds : MonoBehaviour
{
    public Action OnTriggered;
    private float seconds;
    private float time;

    public void SetTempo(float seconds)
    {
        this.seconds = seconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= seconds)
        {
            OnTriggered?.Invoke();
            time = 0;
        }
        time += Time.deltaTime;
    }
}
