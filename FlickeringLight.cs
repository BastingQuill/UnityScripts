using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light candleLight; // Referencia a la luz que quieres que parpadee
    public float minIntensity = 0.5f; // Intensidad mínima de la luz
    public float maxIntensity = 1f; // Intensidad máxima de la luz
    public float flickerSpeed = 5f; // Velocidad del parpadeo

    public float minYOffset = -0.1f; // Desplazamiento mínimo en el eje Y
    public float maxYOffset = 0.1f; // Desplazamiento máximo en el eje Y
    public float moveSpeed = 1f; // Velocidad del movimiento en el eje Y

    private float targetIntensity;
    private float targetYOffset;
    private Vector3 initialPosition;

    private void Start()
    {
        if (candleLight == null)
        {
            candleLight = GetComponent<Light>();
        }

        // Inicializa la intensidad con un valor aleatorio
        candleLight.intensity = Random.Range(minIntensity, maxIntensity);
        targetIntensity = Random.Range(minIntensity, maxIntensity);

        // Guarda la posición inicial de la luz
        initialPosition = transform.localPosition;

        // Inicializa el desplazamiento en Y
        targetYOffset = Random.Range(minYOffset, maxYOffset);
    }

    private void Update()
    {
        if (candleLight == null) return;

        // Interpola la intensidad actual hacia la intensidad objetivo
        candleLight.intensity = Mathf.Lerp(candleLight.intensity, targetIntensity, flickerSpeed * Time.deltaTime);

        // Si la intensidad actual está cerca de la objetivo, elige una nueva intensidad objetivo
        if (Mathf.Abs(candleLight.intensity - targetIntensity) < 0.05f)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }

        // Interpola la posición en Y hacia el desplazamiento objetivo
        float currentYOffset = Mathf.Lerp(transform.localPosition.y - initialPosition.y, targetYOffset, moveSpeed * Time.deltaTime);
        transform.localPosition = new Vector3(initialPosition.x, initialPosition.y + currentYOffset, initialPosition.z);

        // Si el desplazamiento en Y está cerca del objetivo, elige un nuevo desplazamiento objetivo
        if (Mathf.Abs(currentYOffset - targetYOffset) < 0.01f)
        {
            targetYOffset = Random.Range(minYOffset, maxYOffset);
        }
    }
}