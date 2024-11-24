using TMPro;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    private int points = 0;
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
    }

    public void UpdatePointsDisplay()
    {
        if (pointsText != null)
        {
            pointsText.text = "Puntos: " + points.ToString();
        }
    }
}
