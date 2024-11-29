using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objetivo;       // El coche que la cámara debe seguir
    public float altura = 5f;        // Altura de la cámara respecto al coche
    public float distancia = 10f;    // Distancia de la cámara detrás del coche
    public float suavizado = 0.1f;   // Suavizado del movimiento de la cámara

    private Vector3 offset;          // Offset de la cámara respecto al coche

    void Start()
    {
        // Inicializa el offset con la altura y distancia deseada detrás del coche
        offset = new Vector3(0, altura, -distancia);
    }
    void FixedUpdate()
    {
        if (objetivo == null) return;

        Vector3 posicionDeseada = objetivo.position + objetivo.rotation * offset;
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
        transform.LookAt(objetivo.position + Vector3.up * 1.5f);
    }

    void LateUpdate()
    {
        // Si no se asigna un objetivo, no hace nada
        if (objetivo == null) return;

        // Calcula la posición detrás del coche usando su rotación
        Vector3 posicionDeseada = objetivo.position + objetivo.rotation * offset;

        // Suaviza el movimiento de la cámara hacia la posición deseada
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, suavizado);

        // Hace que la cámara siempre mire al coche desde arriba
        transform.LookAt(objetivo.position + Vector3.up * 1.5f);  // Ajusta el punto de mirada ligeramente encima del coche
    }
}
