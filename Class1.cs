public class CarMovement : MonoBehaviour
{
    private NavMeshAgent agent;  // El agente de navegación para el coche
    public float movementSpeed = 5f;  // Velocidad de movimiento del coche
    public float avoidanceDistance = 5f; // Distancia de detección de obstáculos

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;

        // Mueve el coche a un destino aleatorio dentro del NavMesh
        SetRandomDestination();
    }

    private void Update()
    {
        // Si el coche ha llegado al destino, selecciona uno nuevo
        if (!agent.pathPending && agent.remainingDistance < 1f)
        {
            SetRandomDestination();
        }

        // Evitar obstáculos mientras se mueve
        AvoidObstacles();
    }

    void SetRandomDestination()
    {
        // Selecciona una posición aleatoria dentro del NavMesh para mover el coche
        Vector3 randomPosition = new Vector3(
            Random.Range(-50f, 50f),
            transform.position.y,
            Random.Range(-50f, 50f)
        );

        // Establece la nueva posición de destino
        agent.SetDestination(randomPosition);
    }

    // Método de evasión de obstáculos utilizando raycasting
    void AvoidObstacles()
    {
        RaycastHit hit;
        // Lanza un raycast hacia adelante para detectar posibles obstáculos
        if (Physics.Raycast(transform.position, transform.forward, out hit, avoidanceDistance))
        {
            if (hit.collider.CompareTag("Obstacle"))
            {
                // Si el raycast detecta un obstáculo, cambia la dirección del coche
                Vector3 newDirection = Vector3.Cross(transform.up, hit.normal); // Calcula una nueva dirección
                agent.SetDestination(transform.position + newDirection * 5f);  // Desplaza al coche para evitar el obstáculo
            }
        }
    }
}
