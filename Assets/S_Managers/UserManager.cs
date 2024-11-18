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
        // Método público para crear el usuario
        if (userInstance != null)
        {
            Debug.LogWarning("El usuario ya ha sido generado.");
            return;
        }

        userInstance = CreateUserOnSpline();
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
}
