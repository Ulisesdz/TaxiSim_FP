using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PenaltyManager : MonoBehaviour
{
    public Image[] penaltyImages;       // Arreglo de imágenes que se ocultan con cada infracción
    public GameObject restoreLivesPanel; // Panel para ofrecer restaurar vidas
    public PointsManager pointsManager; // Referencia al gestor de puntos
    public CarController carController; // Referencia al CarController
    public MusicManager musicManager; // Music Manager
    private int penalties = 0;          // Contador global de infracciones

    void Start()
    {
        // Asegúrate de que el panel de restaurar vidas esté oculto al inicio
        if (restoreLivesPanel != null)
        {
            restoreLivesPanel.SetActive(false); // Panel oculto al inicio
        }
    }


    // Método para registrar una infracción
    public void RegisterPenalty()
    {
        if (penalties < penaltyImages.Length)
        {
            penaltyImages[penalties].gameObject.SetActive(false); // Oculta la imagen correspondiente
            penalties++; // Incrementa el contador de infracciones
            musicManager.PlayPhotoSound();
        }

        // Si todas las vidas se pierden, mostrar la oferta de restaurar
        if (penalties >= penaltyImages.Length)
        {
            ShowRestoreLivesOffer();
        }
    }

    // Método para reiniciar el sistema de penalizaciones
    public void ResetPenalties()
    {
        foreach (var image in penaltyImages)
        {
            image.gameObject.SetActive(true); // Reactiva todas las imágenes
        }
        penalties = 0; // Reinicia el contador
    }

    // Mostrar oferta para restaurar vidas
    private void ShowRestoreLivesOffer()
    {
        if (restoreLivesPanel != null)
        {
            restoreLivesPanel.SetActive(true); // Mostrar el panel
        }

        if (carController != null)
        {
            carController.isControlEnabled = false; // Desactivar controles
        }
    }

    // Método para restaurar vidas si se aceptó la oferta
    public void RestoreLives()
    {
        if (pointsManager != null && pointsManager.GetCurrentPoints() >= 500) 
        {
            pointsManager.SubtractPoints(500); // Restar puntos
            ResetPenalties(); // Restaurar vidas
            Debug.Log("Vidas restauradas a cambio de 500 puntos.");
            // Ocultar el panel de restauración
            if (restoreLivesPanel != null)
            {
                restoreLivesPanel.SetActive(false);
            }

            if (carController != null)
            {
                carController.isControlEnabled = true; // Reactivar controles
            }
        }
        else
        {
            Debug.Log("No tienes suficientes puntos para restaurar vidas.");
        }
    }

    // Método para cancelar la oferta
    public void DeclineRestoreLives()
    {
        SceneManager.LoadScene("Main_menu");
    }
}
