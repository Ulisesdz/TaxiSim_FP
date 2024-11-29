using UnityEngine;

public class PoliceCar : MonoBehaviour
{
    public float detectionRadius = 15f; // Radio de detección en metros
    public float speedLimit = 70f;      // Límite de velocidad en km/h
    public Speedometer speedometer;     // Referencia al velocímetro del taxi
    private PenaltyManager penaltyManager; // Referencia al gestor global de penalizaciones

    public float penaltyCooldown = 4f;  // Tiempo mínimo entre penalizaciones (en segundos)
    private float lastPenaltyTime = -Mathf.Infinity; // Momento en que se impuso la última penalización

    void Start()
    {
        // Encuentra automáticamente el gestor global de penalizaciones en la escena
        penaltyManager = FindObjectOfType<PenaltyManager>();

        if (penaltyManager == null)
        {
            Debug.LogError("PenaltyManager no encontrado en la escena. Asegúrate de que exista.");
        }

        // Asegura que el coche se posiciona adecuadamente sobre el suelo
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            transform.position = hit.point + Vector3.up * 0.1f; // 0.1f para levantarlo un poco
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Verifica si el objeto en el trigger es el taxi
        if (other.CompareTag("Taxi"))
        {
            if (speedometer != null && speedometer.target.velocity.magnitude * 3.6f > speedLimit)
            {
                // Verifica si ha pasado suficiente tiempo desde la última penalización
                if (Time.time - lastPenaltyTime >= penaltyCooldown)
                {
                    HandleSpeedViolation();
                }
            }
        }
    }

    private void HandleSpeedViolation()
    {
        if (penaltyManager != null)
        {
            penaltyManager.RegisterPenalty(); // Notifica la infracción al gestor
            lastPenaltyTime = Time.time; // Actualiza el tiempo de la última penalización
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja el área de detección en la escena para depuración
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
