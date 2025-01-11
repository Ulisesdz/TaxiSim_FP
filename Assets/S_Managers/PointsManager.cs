using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsManager : MonoBehaviour
{
    private int points = 200; // Default starting points
    private string filePath;

    public TextMeshProUGUI pointsText;

    public FileHandler fileHandler; 

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
            points = 0;
            SavePoints();
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

    public void SavePoints()
    {
        try
        {
            fileHandler.Save(filePath, points.ToString());  // Usamos el método de FileHandler
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
            if (fileHandler.Exists(filePath))  // Verificamos si el archivo existe
            {
                string data = fileHandler.Load(filePath);  // Usamos el método de FileHandler
                if (int.TryParse(data, out int loadedPoints))
                {
                    if (loadedPoints > 0)
                    {
                        points = loadedPoints;
                    }
                    else
                    {
                        points = 200;
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
