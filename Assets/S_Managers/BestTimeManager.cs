using UnityEngine;
public class BestTimeManager
{
    private float bestTime = Mathf.Infinity;
    private readonly IFileHandler fileHandler;
    private readonly string filePath;

    public float BestTime => bestTime;

    public BestTimeManager(IFileHandler fileHandler, string filePath)
    {
        this.fileHandler = fileHandler;
        this.filePath = filePath;
        LoadBestTime();
    }

    public void UpdateBestTime(float newTime)
    {
        if (newTime < bestTime)
        {
            bestTime = newTime;
            SaveBestTime();
        }
    }

    private void SaveBestTime()
    {
        fileHandler.Save(filePath, bestTime.ToString("F1"));
    }

    private void LoadBestTime()
    {
        if (fileHandler.Exists(filePath))
        {
            string data = fileHandler.Load(filePath);
            if (float.TryParse(data, out float loadedTime))
            {
                bestTime = loadedTime;
            }
        }
    }
}
