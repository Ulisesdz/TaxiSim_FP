using UnityEngine;

public class CarManager : MonoBehaviour
{
    public UserManager userManager; // Referencia al UserManager
    public float pickUpDistance = 5f; // Distancia mínima para recoger a la persona
    public Canvas markdownCanvas;

    private GameObject targetPerson; // Persona objetivo (generada por UserManager)

    void Start()
    {
        // Intenta obtener la referencia a la persona generada por UserManager
        if (userManager != null)
        {
            targetPerson = userManager.GetUserInstance();
        }
        else
        {
            Debug.LogError("UserManager no asignado en CarManager.");
        }
    }

    void Update()
    {
        targetPerson = userManager.GetUserInstance();
        if (targetPerson != null)
        {
            // Calcula la distancia entre el taxi y la persona
            float distance = Vector3.Distance(transform.position, targetPerson.transform.position);
            // Si están lo suficientemente cerca, recoge a la persona
            if (distance <= pickUpDistance)
            {
                Debug.Log($"Recogiendo a {targetPerson.name}");

                // Desactivar la persona (simula que sube al taxi)
                targetPerson.SetActive(false);

                // Desactivar el Canvas asociado
                if (markdownCanvas != null)
                {
                    markdownCanvas.gameObject.SetActive(false);
                    Debug.Log("Canvas 'Markdown' desactivado.");
                }

                // Limpia la referencia después de recoger a la persona
                targetPerson = null;
            }
        }
    }
}
