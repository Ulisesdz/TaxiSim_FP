using System.IO;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsManager : MonoBehaviour
{
    private int points = 300; // Default starting points
    private string filePath;

    public TextMeshProUGUI pointsText;

    private void Awake()
    {
        // Define the file path where points will be saved
        filePath = Path.Combine(Application.persistentDataPath, "points.txt");
        LoadPoints(); // Load points when the scene starts
        UpdatePointsDisplay(); // Update UI to reflect the loaded points
    }

    public void AddPoints(int amount)
    {
        points += amount;
        UpdatePointsDisplay();
        SavePoints(); // Save the updated points
    }

    public void SubtractPoints(int amount)
    {
        points -= amount;
        UpdatePointsDisplay();
        SavePoints(); // Save the updated points

        if (points < 0)
        {
            Debug.Log($"Has perdido todos los puntos. Fin de la Partida.");
            SceneManager.LoadScene("Main_menu");
        }
    }

    public int GetCurrentPoints()
    {
        return points;
    }

    public void UpdatePointsDisplay()
    {
        if (pointsText != null)
        {
            pointsText.text = "Puntos: " + points.ToString();
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

    private void LoadPoints()
    {
        try
        {
            if (File.Exists(filePath))
            {
                string data = File.ReadAllText(filePath);
                if (int.TryParse(data, out int loadedPoints))
                {
                    if (loadedPoints > 0)
                    {
                        points = loadedPoints;
                    }
                    else
                    {
                        points = 300;
                    }                    
                }
                else
                {
                    Debug.LogWarning("Failed to parse points. Using default value.");
                }
            }
        }
        catch (IOException e)
        {
            Debug.LogError($"Error loading points: {e.Message}");
        }
    }
}
