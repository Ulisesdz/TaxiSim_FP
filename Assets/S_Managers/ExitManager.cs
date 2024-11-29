using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitManager : MonoBehaviour
{
    public GameObject exitPanel;       // Panel que se muestra al pulsar el botón de parar
    public CarController carController; // Referencia al controlador del coche

    void Start()
    {
        // Asegúrate de que el panel de salida esté oculto al inicio
        if (exitPanel != null)
        {
            exitPanel.SetActive(false);
        }
    }

    // Método para mostrar el panel de salida y pausar el juego
    public void ShowExitPanel()
    {
        if (exitPanel != null)
        {
            exitPanel.SetActive(true); // Mostrar el panel de salida
        }

        if (carController != null)
        {
            carController.isControlEnabled = false; // Desactivar controles
        }
    }

    // Método para ocultar el panel de salida y reanudar el juego
    public void ContinueGame()
    {
        if (exitPanel != null)
        {
            exitPanel.SetActive(false); // Ocultar el panel de salida
        }

        if (carController != null)
        {
            carController.isControlEnabled = true; // Reactivar controles
        }
    }

    // Método para ir al menú principal
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main_menu"); // Cargar la escena del menú principal
    }
}

