using UnityEngine;

public class CarPhysics : MonoBehaviour
{
    public float velocidad = 10f;
    public float giro = 50f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Movimiento hacia adelante y atras
        float movimiento = Input.GetAxis("Vertical") * velocidad * Time.deltaTime;
        // Movimiento hacia la izquierda y derecha
        float rotacion = Input.GetAxis("Horizontal") * giro * Time.deltaTime;

        // Mover el coche hacia adelante/atras
        rb.MovePosition(transform.position + transform.forward * movimiento);
        // Rotar el coche
        rb.MoveRotation(transform.rotation * Quaternion.Euler(0f, rotacion, 0f));
    }
}
