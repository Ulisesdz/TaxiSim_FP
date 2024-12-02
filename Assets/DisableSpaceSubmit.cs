using UnityEngine;
using UnityEngine.EventSystems;

public class DisableSpaceSubmit : MonoBehaviour
{
    void Update()
    {
        // Comprueba si el botón Submit (por defecto, Espacio) está activo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Asegúrate de que ningún botón esté actualmente seleccionado
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}
