using TMPro;
using UnityEngine;

public class AvisoUI : MonoBehaviour
{
    public static AvisoUI instancia;
    public TextMeshProUGUI mensajeUI; // Referencia al TextMeshProUGUI para mostrar el mensaje

    void Awake()
    {
        // Asegúrate de que solo haya una instancia de AvisoUI
        if (instancia == null)
        {
            instancia = this;
            Debug.Log("UICREADA");
        }
        else
            Destroy(gameObject); // Asegura que solo exista una instancia
    }

    public void MostrarMensaje(string mensaje)
    {
        if (mensajeUI != null)
        {
            Debug.Log("Mostrando mensaje en pantalla: " + mensaje);  // Agregar este log
            mensajeUI.text = mensaje;
            mensajeUI.enabled = true;
        }
    }

    public void OcultarMensaje()
    {
        if (mensajeUI != null)
        {
            mensajeUI.enabled = false;
        }
    }
}
