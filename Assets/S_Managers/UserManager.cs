using UnityEngine;
using Dreamteck.Splines;

public class UserManager : MonoBehaviour
{
    public GameObject userPrefab;
    public SplineComputer userSpline;
    private GameObject userInstance; // Almacena la referencia del usuario creado
    public RuntimeAnimatorController movementAnimatorController;

    void Start()
    {
        // No creamos el usuario automáticamente al inicio.
    }

    public void CreateUser()
    {
        // Si ya hay un usuario creado y no ha sido entregado al destino, no se crea uno nuevo
        if (userInstance != null)
        {
            Debug.LogWarning("Ya hay un usuario en curso. No se puede crear uno nuevo.");
            return;
        }

        // Crear el nuevo usuario
        userInstance = CreateUserOnSpline();

        // Notificar al CarManager que el usuario ha sido generado
        if (userInstance != null)
        {
            Debug.Log("Nuevo usuario generado. Notificar al marcador.");
        }
    }

    private GameObject CreateUserOnSpline()
    {
        if (userSpline == null || userPrefab == null)
        {
            Debug.LogError("Spline o Prefab no asignados para el usuario.");
            return null;
        }

        Vector3 userPosition = new Vector3(0, 0, 0);
        double startPercent = (double)Random.Range(0f, 1f);
        GameObject user = Instantiate(userPrefab, userPosition, Quaternion.identity);

        // Establece el usuario generado como hijo del objeto que contiene este script
        user.transform.SetParent(transform);

        user.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        user.name = "User";

        var follower = user.GetComponent<SplineFollower>();
        if (follower == null)
        {
            follower = user.AddComponent<SplineFollower>();
        }

        follower.spline = userSpline;
        follower.autoStartPosition = false;
        follower.wrapMode = SplineFollower.Wrap.Loop;
        follower.followSpeed = 0;
        double totalLength = userSpline.CalculateLength();
        float distance = (float)(startPercent * totalLength);
        follower.SetDistance(distance);
        follower.SetPercent(startPercent);

        Debug.Log($"User generado en el spline: percent = {startPercent}, position = {userPosition}");

        var animator = user.GetComponent<Animator>();
        if (animator != null && movementAnimatorController != null)
        {
            animator.runtimeAnimatorController = movementAnimatorController;
        }

        return user;
    }

    public GameObject GetUserInstance()
    {
        return userInstance; // Devuelve correctamente el usuario generado
    }

    // Método para eliminar al usuario una vez haya llegado al destino
    public void RemoveUser()
    {
        if (userInstance != null)
        {
            Destroy(userInstance);
            userInstance = null; // Restablecer la referencia
            Debug.Log("Usuario destruido después de ser entregado.");
        }
    }
}
