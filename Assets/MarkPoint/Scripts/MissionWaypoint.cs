using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionWaypoint : MonoBehaviour
{
    public Image img;                  // Icono del marcador
    public TextMeshProUGUI meter;      // Texto para la distancia
    public Vector3 offset;             // Desplazamiento del marcador
    public float escalaIcono = 0.5f;   // Escala del marcador
    public float alturaExtra = 5f;     // Altura adicional

    private Transform target;          // Objetivo dinámico
    private bool isHeadingDestination;

    private void Start()
    {
        offset.y += alturaExtra;
        SetupTextAlignment();
        AjustarEscalaIcono();
    }

    private void Update()
    {
        if (target == null)
        {
            Debug.Log("El target del marcador es null.");
            return;
        }

        // Convertir posición 3D a coordenadas de pantalla
        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

        // Limitar la posición dentro de los bordes de la pantalla
        RectTransform iconRectTransform = img.GetComponent<RectTransform>();
        float iconWidth = iconRectTransform.rect.width * escalaIcono;
        float iconHeight = iconRectTransform.rect.height * escalaIcono;

        float minX = iconWidth / 2 + 50;
        float maxX = Screen.width - minX;
        float minY = iconHeight / 2 + 25;
        float maxY = Screen.height - minY;

        // Comprobar si el objetivo está detrás del taxi
        if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            // Si el objetivo está detrás del taxi, colocamos el marcador en el lado opuesto
            if (pos.x < Screen.width / 2)
            {
                pos.x = maxX; // Si está detrás y a la izquierda, lo movemos a la derecha
            }
            else
            {
                pos.x = minX; // Si está detrás y a la derecha, lo movemos a la izquierda
            }
        }
        else
        {
            // Si está al frente del taxi, lo mostramos normalmente
            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
        }

        // Actualizamos la posición del marcador en la pantalla
        img.transform.position = pos;

        // Mostrar la distancia en metros
        meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
        //Debug.Log($"Waypoint apunta a las coordenadas del target: {target.position}");
        //Debug.Log($"Waypoint apunta a las coordenadas del transformada: {transform.position}");
    }

    public void SetTarget(Transform newTarget, string manager)
    {
        if (manager == "PASMAN")
        {
            Debug.Log($"Marcador activado y apuntando al destino WAYPOINT. {newTarget.transform.position} {manager}");
            target = newTarget;
        }
        else
        {
            if (manager == "CARMAN2" && isHeadingDestination)
            {
                Debug.Log($"Marcador activado y apuntando al destino WAYPOINT. {newTarget.transform.position} {manager}");
                target = newTarget;
            }
            if (manager == "CARMAN1" && !isHeadingDestination)
            {
                Debug.Log($"Marcador activado y apuntando al destino WAYPOINT. {newTarget.transform.position} {manager}");
                target = newTarget;
            }
        }
    }

    private void SetupTextAlignment()
    {
        meter.alignment = TextAlignmentOptions.Center;
        RectTransform meterRectTransform = meter.GetComponent<RectTransform>();
        meterRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        meterRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        meterRectTransform.anchoredPosition = Vector2.zero;
    }

    private void AjustarEscalaIcono()
    {
        RectTransform rectTransform = img.GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(escalaIcono, escalaIcono, escalaIcono);

        RectTransform meterRectTransform = meter.GetComponent<RectTransform>();
        meterRectTransform.localScale = new Vector3(escalaIcono, escalaIcono, escalaIcono);
    }

    public Transform GetTarget()
    {
        return target;
    }

    public void IsHeadingDestination(bool isHeading)
    {
        isHeadingDestination = isHeading;
    }
}

