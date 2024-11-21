using UnityEngine;
using Dreamteck.Splines;
using TMPro;

public class CarManager : MonoBehaviour
{
    public UserManager userManager; // Referencia al UserManager
    public float pickUpDistance = 5f; // Distancia mínima para recoger a la persona
    public Canvas markdownCanvas; // Referencia al marcador 2D
    public SplineComputer destinationSpline; // Spline de destino
    public GameObject destinationMarker; // Marcador de destino (no es prefab, es un GameObject ya en la jerarquía)
    public MissionWaypoint waypoint; // Referencia al marcador dinámico
    public float arrivalDistance = 2f; // Distancia mínima para llegar al destino

    public TextMeshProUGUI timerText; // TextMeshProUGUI para mostrar el tiempo
    public TextMeshProUGUI pointsText; // TextMeshProUGUI para mostrar los puntos
    private GameObject targetPerson; // Persona objetivo
    private bool isHeadingToDestination = false; // Si el taxi lleva al cliente
    private bool isWaitingForNewPassenger = false; // Si estamos esperando un nuevo cliente

    private float maxTimeToDestination = 15f; // Tiempo máximo en segundos para llegar al destino
    private float timeRemaining; // Contador de tiempo para llegar al destino

    private int points = 0; // Puntos acumulados

    private bool canCollide = true; // Indica si el coche puede detectar colisiones
    private float collisionCooldown = 2f; // Tiempo de espera entre colisiones
    private float collisionTimer = 0f; // Temporizador para manejar el cooldown

    void Start()
    {
        targetPerson = null; // No hay persona al inicio

        // Asegúrate de que el marcador de destino esté desactivado al inicio
        if (destinationMarker != null)
        {
            destinationMarker.SetActive(false);
        }
        else
        {
            Debug.LogError("El marcador de destino no está asignado.");
        }

        // Ocultar el marcador al inicio
        if (markdownCanvas != null)
        {
            markdownCanvas.gameObject.SetActive(false);
        }

        // Inicializa el tiempo restante
        timeRemaining = maxTimeToDestination;

        // Mostrar el tiempo restante con 1 decimal al inicio
        if (timerText != null)
        {
            timerText.gameObject.SetActive(false); // Asegúrate de que el texto no esté visible al inicio
            timerText.text = timeRemaining.ToString("F1") + "s";
        }

        // Mostrar los puntos al inicio
        UpdatePointsDisplay();
    }

    void Update()
    {
        if (!isHeadingToDestination)
        {
            // Comprobar si hay un cliente para recoger
            targetPerson = userManager.GetUserInstance();

            if (targetPerson != null)
            {
                // Activar el marcador cuando se genera el usuario
                if (markdownCanvas != null && !markdownCanvas.gameObject.activeSelf)
                {
                    markdownCanvas.gameObject.SetActive(true);
                    Debug.Log("Marcador activado y apuntando al usuario.");
                }

                // Actualizar el objetivo del marcador al usuario
                if (waypoint != null && waypoint.GetTarget() != targetPerson.transform)
                {
                    waypoint.SetTarget(targetPerson.transform);
                }

                // Comprobar si el taxi está lo suficientemente cerca del cliente
                float distance = Vector3.Distance(transform.position, targetPerson.transform.position);

                if (distance <= pickUpDistance)
                {
                    Debug.Log($"Recogiendo a {targetPerson.name}");

                    // Generar el punto de destino
                    GenerateDestinationPoint();

                    // Desactivar al cliente
                    targetPerson.SetActive(false);

                    // Cambiar el estado para dirigirse al destino
                    isHeadingToDestination = true;

                    // Reiniciar el contador de tiempo
                    timeRemaining = maxTimeToDestination;

                    // Mostrar el temporizador
                    if (timerText != null)
                    {
                        timerText.gameObject.SetActive(true); // Activamos el texto del temporizador
                    }
                }
            }
        }
        else
        {
            // Verificar la distancia al marcador de destino
            if (destinationMarker != null)
            {
                float distanceToDestination = Vector3.Distance(transform.position, destinationMarker.transform.position);

                if (distanceToDestination <= arrivalDistance)
                {
                    // Desactivar el marcador y el markdown cuando el taxi llegue al destino
                    if (markdownCanvas != null)
                    {
                        markdownCanvas.gameObject.SetActive(false);
                    }

                    if (destinationMarker != null)
                    {
                        destinationMarker.SetActive(false);
                    }

                    // Terminar la misión
                    Debug.Log("¡Has llegado al destino!");

                    // Verificar si llegamos a tiempo
                    if (timeRemaining > 0)
                    {
                        // Sumar puntos si llegamos a tiempo
                        points += 200;
                        Debug.Log("¡Llegaste a tiempo! Puntos sumados.");
                    }
                    else
                    {
                        // Restar puntos si no llegamos a tiempo
                        points -= 100;
                        Debug.Log("¡No llegaste a tiempo! Puntos restados.");
                    }

                    // Actualizar los puntos en pantalla
                    UpdatePointsDisplay();

                    // Cambiar el estado para permitir la búsqueda de un nuevo cliente
                    isHeadingToDestination = false;

                    // Eliminar el usuario entregado
                    userManager.RemoveUser();

                    // Iniciar el contador para esperar un nuevo cliente
                    isWaitingForNewPassenger = true;

                    // Ocultar el temporizador
                    if (timerText != null)
                    {
                        timerText.gameObject.SetActive(false); // Desactivar el texto del temporizador
                    }
                }
            }

            // Decrementar el tiempo restante
            if (isHeadingToDestination && timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime; // Reduce el tiempo restante

                // Actualizar el texto del temporizador
                if (timerText != null)
                {
                    timerText.text = timeRemaining.ToString("F1") + "s"; // Mostrar el tiempo con 1 decimal
                }

                // Si se acabó el tiempo, tomar alguna acción
                if (timeRemaining <= 0)
                {
                    Debug.Log("¡El tiempo se ha agotado! No llegaste al destino a tiempo.");
                }
            }
        }

        // Permitir la creación de un nuevo cliente después de la espera
        if (isWaitingForNewPassenger && userManager.GetUserInstance() == null)
        {
            Debug.Log("Esperando un nuevo cliente...");
            isWaitingForNewPassenger = false;
        }

        // Manejo del cooldown de colisiones
        if (!canCollide)
        {
            collisionTimer += Time.deltaTime;
            if (collisionTimer >= collisionCooldown)
            {
                collisionTimer = 0f;
                canCollide = true; // Permitimos detectar colisiones nuevamente
            }
        }
    }

    private void GenerateDestinationPoint()
    {
        if (destinationSpline == null)
        {
            Debug.LogError("Spline de destino no asignado.");
            return;
        }

        // Generar un punto aleatorio en el spline de destino
        double randomPercent = Random.Range(0f, 1f);
        Vector3 generatedPoint = destinationSpline.EvaluatePosition(randomPercent);

        if (destinationMarker != null)
        {
            // Coloca el marcador de destino en el punto generado
            destinationMarker.transform.position = generatedPoint;

            // Asegúrate de que el marcador esté activado
            destinationMarker.SetActive(true);

            Debug.Log($"Punto de destino generado en el spline: {generatedPoint}");
        }

        // Actualizar el marcador dinámico para apuntar al destino
        if (waypoint != null)
        {
            waypoint.SetTarget(destinationMarker.transform);
        }
    }

    private void UpdatePointsDisplay()
    {
        // Actualiza el texto de los puntos en pantalla
        if (pointsText != null)
        {
            pointsText.text = "Puntos: " + points.ToString(); // Muestra los puntos en el UI
        }
    }

    // Detectar colisiones
    private void OnCollisionEnter(Collision collision)
    {
        if (canCollide)
        {
            // Restar puntos por colisión
            points -= 50;
            Debug.Log("¡Colisión! Se restan 50 puntos.");

            // Actualizar el texto de los puntos
            UpdatePointsDisplay();

            // Iniciar el cooldown
            canCollide = false;
        }
    }
}

