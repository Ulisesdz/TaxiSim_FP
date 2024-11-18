using UnityEngine;

public class NPCNotifier : MonoBehaviour
{
    public float rangoTaxi = 3f;  // Distancia para detectar el taxi
    public GameObject taxi;       // Referencia al taxi en la escena
    private bool avisoActivo = false;

    void Start()
    {
        if (taxi == null)
        {
            taxi = GameObject.FindWithTag("Taxi"); // Busca un objeto con el tag "Taxi"
        }
    }

    void Update()
    {
        if (avisoActivo && taxi != null)
        {
            // Verificar si el taxi está lo suficientemente cerca
            float distancia = Vector3.Distance(taxi.transform.position, transform.position);
            if (distancia <= rangoTaxi)
            {
                RecogerPasajero(); // El taxi recoge al pasajero
            }
        }
    }

    // Método llamado por el botón para activar el aviso
    public void ActivarAvisoDesdeBoton()
    {
        Debug.Log("Botón presionado: ActivarAvisoDesdeBoton llamado.");
        avisoActivo = true;
        AvisoUI.instancia?.MostrarMensaje("¡El taxi ha sido llamado! Ve a recoger al pasajero.");
    }

    // Método para ocultar el aviso y destruir al personaje
    public void RecogerPasajero()
    {
        avisoActivo = false;
        AvisoUI.instancia?.OcultarMensaje();
        Destroy(gameObject); // Destruir al NPC
    }
}
