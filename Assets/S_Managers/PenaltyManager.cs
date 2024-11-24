using UnityEngine;
using UnityEngine.UI;

public class PenaltyManager : MonoBehaviour
{
    public Image[] penaltyImages;       // Arreglo de imágenes que se ocultan con cada infracción
    private int penalties = 0;          // Contador global de infracciones

    // Método para registrar una infracción
    public void RegisterPenalty()
    {
        if (penalties < penaltyImages.Length)
        {
            penaltyImages[penalties].gameObject.SetActive(false); // Oculta la imagen correspondiente
            penalties++; // Incrementa el contador de infracciones
        }
        else
        {
            Debug.Log("Todas las imágenes de penalización ya están ocultas.");
        }
    }

    // Método para reiniciar el sistema de penalizaciones (opcional)
    public void ResetPenalties()
    {
        foreach (var image in penaltyImages)
        {
            image.gameObject.SetActive(true); // Reactiva todas las imágenes
        }
        penalties = 0; // Reinicia el contador
    }
}
