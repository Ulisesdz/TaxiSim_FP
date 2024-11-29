using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public float maxTimeToDestination = 30f;
    private float timeRemaining;

    public TextMeshProUGUI timerText;

    public void StartTimer()
    {
        timeRemaining = maxTimeToDestination;
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }
    }

    public void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = timeRemaining.ToString("F1") + "s";
            }
        }
    }

    public void HideTimer()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false); 
            timerText.text = timeRemaining.ToString("F1") + "s";
        }
    }

    public bool HasTimeLeft()
    {
        return timeRemaining > 0;
    }
}