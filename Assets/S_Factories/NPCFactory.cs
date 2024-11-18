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

        return npc;
    }
}
