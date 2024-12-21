using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public PointsManager pointsManager;  // Referencia al PointsManager para actualizar los puntos
    private bool canCollide = true;      // Si las colisiones están habilitadas
    private float collisionCooldown = 2f; // Tiempo entre colisiones
    private float collisionTimer = 0f;   // Temporizador para el cooldown
    public MusicManager musicManager; // Music Manager

    void Update()
    {
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

    // Detectar colisiones
    private void OnCollisionEnter(Collision collision)
    {
        if (canCollide)
        {
            // Restar puntos por colisión
            pointsManager.SubtractPoints(50);
            musicManager.PlayCrashSound();
            Debug.Log("¡Colisión! Se restan 50 puntos.");

            // Actualizar los puntos en pantalla
            pointsManager.UpdatePointsDisplay();

            // Iniciar el cooldown
            canCollide = false;
        }
    }
}
