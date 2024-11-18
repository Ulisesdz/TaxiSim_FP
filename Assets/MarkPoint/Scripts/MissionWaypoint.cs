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

    public UserManager userManager;    // Referencia al UserManager
    private Transform target;          // Objetivo dinámico

    private void Start()
    {
        // Asegurarse de que el marcador no sea visible al inicio
        SetWaypointVisibility(false);

        offset.y += alturaExtra;
        SetupTextAlignment();
        AjustarEscalaIcono();
    }

    private void Update()
    {
        if (target == null)
        {
            // Intentar obtener el target dinámicamente
            if (userManager != null)
            {
                GameObject user = userManager.GetUserInstance();
                if (user != null)
                {
                    target = user.transform;
                    Debug.Log("Waypoint configurado para apuntar al User generado.");

                    // Hacer visible el marcador
                    SetWaypointVisibility(true);
                }
            }

            // Si aún no hay un target, detener aquí
            if (target == null) return;
        }

        // Convertir posición 3D a coordenadas de pantalla
        Vector2 pos = Camera.main.WorldToScreenPoint(target.position + offset);

        RectTransform iconRectTransform = img.GetComponent<RectTransform>();
        float iconWidth = iconRectTransform.rect.width * escalaIcono;
        float iconHeight = iconRectTransform.rect.height * escalaIcono;

        float minX = iconWidth / 2 + 50;
        float maxX = Screen.width - minX;
        float minY = iconHeight / 2 + 25;
        float maxY = Screen.height - minY;

        if (Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            pos.x = pos.x < Screen.width / 2 ? maxX : minX + 25;
        }

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        img.transform.position = pos;

        meter.text = ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
    }

    private void SetWaypointVisibility(bool visible)
    {
        img.gameObject.SetActive(visible);
        meter.gameObject.SetActive(visible);
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
}
