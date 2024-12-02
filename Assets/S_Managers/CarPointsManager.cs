using System.IO;
using TMPro;
using UnityEngine;

public class CarPointsManager : MonoBehaviour
{
    private int points;
    private string filePath;

    public TextMeshProUGUI pointsText;

    private void Awake()
    {
        // Define the file path where points are saved
        filePath = Path.Combine(Application.persistentDataPath, "points.txt");
        LoadPoints(); // Load points when the scene starts
        UpdatePointsDisplay(); // Update the UI to reflect the loaded points
    }

    private void OnDestroy()
    {
        SavePoints(); // Save points when exiting the scene
    }

    private void LoadPoints()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string data = File.ReadAllText(filePath);
                if (int.TryParse(data, out int loadedPoints))
                {
                    points = loadedPoints;
                }
                else
                {
                    Debug.LogWarning("Failed to parse points. Using default value.");
                }
            }
            else
            {
                Debug.LogWarning("Points file not found. Using default value.");
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error loading points: {e.Message}");
        }
    }

    private void SavePoints()
    {
        try
        {
            File.WriteAllText(filePath, points.ToString());
        }
        catch (IOException e)
        {
            Debug.LogError($"Error saving points: {e.Message}");
        }
    }

    public void UpdatePointsDisplay()
    {
        if (pointsText != null)
        {
            pointsText.text = "Puntos: " + points.ToString();
        }
    }
    public int GetCurrentPoints()
    {
        return points;
    }
    public void AddPoints(int amount)
    {
        points += amount;
        UpdatePointsDisplay();
    }

    public void SubtractPoints(int amount)
    {
        points -= amount;
        UpdatePointsDisplay();
    }
}

