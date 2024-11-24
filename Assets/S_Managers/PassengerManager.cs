using UnityEngine;
using Dreamteck.Splines;

public class PassengerManager : MonoBehaviour
{
    public float pickUpDistance = 5f;
    public float arrivalDistance = 2f;

    public UserManager userManager;
    public Canvas markdownCanvas;
    public SplineComputer destinationSpline;
    public GameObject destinationMarker;
    public MissionWaypoint waypoint;
    public GameObject destinationParticles;

    private void Start()
    {
        // Ocultar el marcador y las partículas al inicio
        if (markdownCanvas != null) markdownCanvas.gameObject.SetActive(false);
        if (destinationMarker != null) destinationMarker.SetActive(false);
        if (destinationParticles != null) destinationParticles.SetActive(false);
    }

    public bool TryPickUpPassenger(Transform taxiTransform)
    {
        GameObject targetPerson = GetTargetPerson();
        if (targetPerson == null) return false;

        float distance = Vector3.Distance(taxiTransform.position, targetPerson.transform.position);
        if (distance <= pickUpDistance)
        {
            Debug.Log($"Recogiendo a {targetPerson.name}");
            GenerateDestinationPoint();
            targetPerson.SetActive(false); // Desactivar al cliente
            if (markdownCanvas != null) markdownCanvas.gameObject.SetActive(true);
            return true;
        }

        return false;
    }

    public bool TryLeavePassenger(Transform taxiTransform)
    {
        float distanceToDestination = Vector3.Distance(taxiTransform.position, destinationMarker.transform.position);

        if (distanceToDestination <= arrivalDistance)
        {
            if (markdownCanvas != null) markdownCanvas.gameObject.SetActive(false);
            if (destinationMarker != null) destinationMarker.SetActive(false);
            if (destinationParticles != null) destinationParticles.SetActive(false);

            Debug.Log("¡Has llegado al destino!");
            userManager.RemoveUser();
            return true;
        }

        return false;
    }

    private void GenerateDestinationPoint()
    {
        if (destinationSpline == null)
        {
            Debug.LogError("Spline de destino no asignado.");
            return;
        }

        double randomPercent = Random.Range(0f, 1f);
        Vector3 generatedPoint = destinationSpline.EvaluatePosition(randomPercent);

        if (destinationMarker != null)
        {
            destinationMarker.transform.position = generatedPoint;
            destinationMarker.SetActive(true);
        }

        if (destinationParticles != null)
        {
            destinationParticles.transform.position = generatedPoint;
            destinationParticles.SetActive(true);
        }

        if (waypoint != null) waypoint.SetTarget(destinationMarker.transform);
    }

    public GameObject GetTargetPerson()
    {
        return userManager.GetUserInstance();
    }
}
