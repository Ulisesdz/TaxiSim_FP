using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class PointsManager : MonoBehaviour
{
    private int points = 300;
    public TextMeshProUGUI pointsText;

    public void AddPoints(int amount)
    {
        points += amount;
        UpdatePointsDisplay();
    }

    public void SubtractPoints(int amount)
    {
        points -= amount;
        UpdatePointsDisplay();
        // Si no hay puntos
        if (points < 0)
        {
            Debug.Log($"Has perdido todos los puntos. Fin de la Partida.");
            SceneManager.LoadScene("Main_menu");
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

}
