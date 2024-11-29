using UnityEngine;

public class ChronoTimer
{
    public float TimeElapsed { get; private set; }
    public bool IsRunning { get; private set; }

    public void Start()
    {
        IsRunning = true;
        TimeElapsed = 0f;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void Update()
    {
        if (IsRunning)
        {
            TimeElapsed += Time.deltaTime;
        }
    }

    public void Reset()
    {
        TimeElapsed = 0f;
    }
}
