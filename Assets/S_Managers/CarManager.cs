using UnityEngine;
using UnityEngine.UI;

public class CarManager : MonoBehaviour
{
    public Button searchButton;  // Referencia al botón de buscar cliente

    public PassengerManager passengerManager; // Manager de Pasajeros
    public TimerManager timerManager; // Manager del temporizador
    public PointsManager pointsManager; // Manager de puntos

    public Canvas markdownCanvas; // Referencia al marcador 2D
    public GameObject destinationMarker; // Marcador de destino (no es prefab, es un GameObject ya en la jerarquía)
    public MissionWaypoint waypoint; // Referencia al marcador dinámico

    private bool isHeadingToDestination = false; // Si el taxi lleva al cliente
    private bool isWaitingForNewPassenger = false; // Si estamos esperando un nuevo cliente
    private GameObject targetPerson; // Persona objetivo


    void Start()
    {
        targetPerson = null; // No hay persona al inicio

        // Asegúrate de que el marcador de destino esté desactivado al inicio
        if (destinationMarker != null)
        {
            destinationMarker.SetActive(false);
        }

        // Ocultar el marcador al inicio
        if (markdownCanvas != null)
        {
            markdownCanvas.gameObject.SetActive(false);
        }

        // Mostrar el tiempo restante con 1 decimal al inicio
        if (timerManager != null)
        {
            timerManager.HideTimer();
        }

        // Mostrar los puntos
        pointsManager.UpdatePointsDisplay();
    }

    void Update()
    {
        if (!isHeadingToDestination)
        {
            // Comprobar si hay un cliente para recoger
            targetPerson = passengerManager.GetTargetPerson();

            if (targetPerson != null)
            {
                // Desactivar el botón de búsqueda
                searchButton.interactable = false;

                // Activar el marcador cuando se genera el usuario
                if (markdownCanvas != null && !markdownCanvas.gameObject.activeSelf)
                {
                    markdownCanvas.gameObject.SetActive(true);
                }

                // Actualizar el objetivo del marcador al usuario
                if (waypoint != null && waypoint.GetTarget() != targetPerson.transform)
                {
                    waypoint.SetTarget(targetPerson.transform);
                    Debug.Log("Marcador activado y apuntando al usuario.");
                }

                // Comprobar si el taxi está lo suficientemente cerca del cliente
                bool pickup = passengerManager.TryPickUpPassenger(transform);
                if (pickup == true)
                {
                    // Cambiar el estado para dirigirse al destino
                    isHeadingToDestination = true;

                    // Activo el timer
                    timerManager.StartTimer();
                }
            }
        }
        else
        {
            // Verificar la distancia al marcador de destino
            if (destinationMarker != null)
            {
                // Comprobar si el taxi está lo suficientemente cerca del destino
                bool pickup = passengerManager.TryLeavePassenger(transform);
                if (pickup == true)
                {
                    // Verificar si llegamos a tiempo
                    if (timerManager.HasTimeLeft())
                    {
                        // Sumar puntos si llegamos a tiempo
                        pointsManager.AddPoints(200);
                        Debug.Log("¡Llegaste a tiempo! Puntos sumados.");
                    }
                    else
                    {
                        // Sumar menos puntos si no llegamos a tiempo
                        pointsManager.AddPoints(100);
                        Debug.Log("¡No llegaste a tiempo! Puntos restados.");
                    }

                    // Actualizar los puntos en pantalla
                    pointsManager.UpdatePointsDisplay();

                    // Cambiar el estado para permitir la búsqueda de un nuevo cliente
                    isHeadingToDestination = false;

                    // Iniciar el contador para esperar un nuevo cliente
                    isWaitingForNewPassenger = true;

                    // Rehabilitar el botón de búsqueda una vez se haya entregado el pasajero
                    searchButton.interactable = true;

                    // Ocultar el temporizador
                    if (timerManager != null)
                    {
                        timerManager.timerText.gameObject.SetActive(false); // Desactivar el texto del temporizador
                    }
                }
            }

            // Decrementar el tiempo restante si está en camino
            if (isHeadingToDestination)
            {
                timerManager.UpdateTimer();
            }
        }

        // Permitir la creación de un nuevo cliente después de la espera
        if (isWaitingForNewPassenger && passengerManager.GetTargetPerson() == null)
        {
            Debug.Log("Esperando un nuevo cliente...");
            isWaitingForNewPassenger = false;
        }
    }
}
