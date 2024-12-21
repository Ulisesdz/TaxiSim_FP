using Dreamteck.Splines;
using UnityEngine;

public abstract class NPCFactory : MonoBehaviour
{
    public GameObject prefab; // Prefab del NPC
    public RuntimeAnimatorController movementAnimatorController; // Animator Controller del Movement

    // Método para crear un NPC y asignarle un spline y un Animator (Movement)
    public GameObject CreateNPC(Vector3 position, SplineComputer splineComputer, double startPercent, float speedNPC)
    {
        // Instanciamos el NPC
        GameObject npc = Instantiate(prefab, position, Quaternion.identity);

        // Establecemos el padre del NPC al transform de la fábrica (esto lo coloca en el contenedor correcto)
        npc.transform.SetParent(this.transform);

        // Asignamos la escala deseada (1.5 en los tres ejes)
        npc.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        // Asigno el RigidBody
        Rigidbody rb = npc.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = npc.AddComponent<Rigidbody>();
        }
        rb.mass = 1000f;
        rb.drag = 1f;
        rb.angularDrag = 1f;
        rb.centerOfMass = new Vector3(0, -2, 0);

        // Verificamos si el NPC tiene el componente SplineFollower
        var follower = npc.GetComponent<SplineFollower>();
        if (follower == null)
        {
            // Si no tiene SplineFollower, lo agregamos
            follower = npc.AddComponent<SplineFollower>();
        }

        // Desactivamos autoStartPosition para evitar que el NPC se reinicie al inicio del spline
        follower.autoStartPosition = false;

        // Configuramos el SplineFollower
        follower.spline = splineComputer;
        follower.wrapMode = SplineFollower.Wrap.Loop;

        // Modifico la velocidad del NPC
        follower.followSpeed = speedNPC;

        // Calculamos la distancia total del spline
        double totalLength = splineComputer.CalculateLength();
        float distance = (float)(startPercent * totalLength);  // Convertimos startPercent a distancia

        // Establecemos la distancia y el porcentaje inicial
        follower.SetDistance(distance);   // Establecemos la distancia calculada
        follower.SetPercent(startPercent);  // Establecemos el porcentaje también

        // Obtener los valores actuales del SplineFollower
        double currentPercent = follower.GetCurrentPercent();
        float currentDistance = follower.GetCurrentDistance();

        Debug.Log($"NPC Creado: percent: {currentPercent}, distance: {currentDistance}");

        // Asignamos el Animator Controller al componente Animator del NPC
        var animator = npc.GetComponent<Animator>();
        if (animator != null && movementAnimatorController != null)
        {
            animator.runtimeAnimatorController = movementAnimatorController;
        }

        // Verificar y agregar un Collider al NPC
        var collider = npc.GetComponent<Collider>();
        if (collider == null)
        {
            // Asumiendo que es un NPC humanoide, usamos un CapsuleCollider
            var capsuleCollider = npc.AddComponent<CapsuleCollider>();
            capsuleCollider.center = new Vector3(0, 1, 0); // Ajustar la posición del centro
            capsuleCollider.height = 2.0f; // Altura del collider
            capsuleCollider.radius = 0.5f; // Radio del collider
        }

        return npc;
    }
}
