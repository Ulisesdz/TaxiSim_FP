using UnityEngine;

public class CarPhysics : MonoBehaviour
{
    public float velocidad = 10f;  // Velocidad de aceleración
    public float giro = 50f;  // Velocidad de giro
    public float velocidadMaxima = 80f;  // Límite de velocidad máxima
    private Rigidbody rb;

    // Variables para controlar la aceleración y la rotación suave
    public float aceleracionSmooth = 0.1f;
    public float rotacionSmooth = 0.1f;
    private float velocidadActual = 0f;
    private float rotacionActual = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Obtén los valores de entrada
        float inputMovimiento = Input.GetAxis("Vertical");
        float inputRotacion = Input.GetAxis("Horizontal");

        // Suavizar la aceleración del coche
        velocidadActual = Mathf.Lerp(velocidadActual, inputMovimiento * velocidad, aceleracionSmooth);

        // Limitar la velocidad máxima
        Vector3 velocidadDeseada = transform.forward * velocidadActual;
        velocidadDeseada = Vector3.ClampMagnitude(velocidadDeseada, velocidadMaxima);  // Limitar la velocidad máxima

        // Establecer la velocidad del Rigidbody
        rb.velocity = new Vector3(velocidadDeseada.x, rb.velocity.y, velocidadDeseada.z);  // Mantener la componente y intacta para la física

        // Suavizar la rotación del coche
        rotacionActual = Mathf.Lerp(rotacionActual, inputRotacion * giro, rotacionSmooth);

        // Aplicar rotación
        float rotacionFinal = rotacionActual * Time.deltaTime;
        Quaternion rotacionDeseada = Quaternion.Euler(0f, rotacionFinal, 0f);
        rb.MoveRotation(rb.rotation * rotacionDeseada);
    }
}
