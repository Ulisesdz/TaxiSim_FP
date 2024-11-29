// ChronoManager.cs
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChronoManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI bestTimeText;
    public Transform startPoint;
    public Transform endPoint;
    public Transform taxi;
    public GameObject restoreLevelPanel;
    public CarController carController;
    public float detectionRadius = 20f;

    private ChronoTimer chronoTimer;
    private BestTimeManager bestTimeManager;
    private string bestTimeFilePath;

    private void Start()
    {
        bestTimeFilePath = System.IO.Path.Combine(Application.persistentDataPath, "bestTime.txt");
        bestTimeManager = new BestTimeManager(new FileHandler(), bestTimeFilePath);
        chronoTimer = new ChronoTimer();

        UpdateBestTimeUI();

        if (restoreLevelPanel != null)
        {
            restoreLevelPanel.SetActive(false);

        }
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false); 
        }
    }

    private void Update()
    {
        DetectStartPoint();
        DetectEndPoint();

        chronoTimer.Update();

        if (timerText != null && chronoTimer.IsRunning)
        {
            timerText.text = $"{chronoTimer.TimeElapsed:F1}s";
        }
    }

    private void DetectStartPoint()
    {
        if (!chronoTimer.IsRunning && Vector3.Distance(taxi.position, startPoint.position) <= detectionRadius)
        {
            StartChrono();
        }
    }

    private void DetectEndPoint()
    {
        if (chronoTimer.IsRunning && Vector3.Distance(taxi.position, endPoint.position) <= detectionRadius)
        {
            StopChrono();
        }
    }

    private void StartChrono()
    {
        chronoTimer.Start();
        if (timerText != null)
        {
            timerText.gameObject.SetActive(true);
        }
    }

    private void StopChrono()
    {
        chronoTimer.Stop();
        bestTimeManager.UpdateBestTime(chronoTimer.TimeElapsed);
        UpdateBestTimeUI();
        ShowRestoreLevelOffer();
    }

    private void UpdateBestTimeUI()
    {
        if (bestTimeText != null)
        {
            bestTimeText.text = bestTimeManager.BestTime == Mathf.Infinity
                ? "Mejor tiempo: -"
                : $"Mejor tiempo: {bestTimeManager.BestTime:F1}s";
        }
    }

    private void ShowRestoreLevelOffer()
    {
        if (restoreLevelPanel != null)
        {
            restoreLevelPanel.SetActive(true);
        }

        if (carController != null)
        {
            carController.isControlEnabled = false;
        }
    }

    public void DeclineRestoreLevel()
    {
        SceneManager.LoadScene("Main_menu");
    }

    public void AcceptRestoreLevel()
    {
        SceneManager.LoadScene("Highway_scene");
    }
}
