using UnityEngine;

public class DynController : MonoBehaviour
{
    [SerializeField] private float velocidad = 10f;       // Velocidad del proyectil
    [SerializeField] private float dano = 10f;            // Daño que hace el proyectil
    [SerializeField] private float tiempoDeVida = 5f;     // Tiempo máximo antes de volver al pool
    private float velocidadRotacion = 360f;

    private Vector3 direccion;                            // Dirección del proyectil
    private float tiempoTranscurrido = 0f;                // Tiempo en vuelo
    private EnemyProjectilePool pool;                     // Referencia al pool de proyectiles enemigos

    // Inicializar el proyectil con dirección, objetivo y pool
    public void Initialize(Vector3 nuevaDireccion, float dano, EnemyProjectilePool pool)
    {
        direccion = nuevaDireccion.normalized; // Normalizar dirección
        this.dano = dano;                      // Asignar daño
        this.pool = pool;                      // Asignar pool
        tiempoTranscurrido = 0f;               // Reiniciar el tiempo de vida
    }

    private void Update()
    {
        // Mover el proyectil en la dirección establecida
        transform.position += direccion * velocidad * Time.deltaTime;

        // Rotar el proyectil sobre el eje Z
        transform.Rotate(0, 0, velocidadRotacion * Time.deltaTime);

        // Devolver al pool si excede el tiempo de vida
        tiempoTranscurrido += Time.deltaTime;
        if (tiempoTranscurrido >= tiempoDeVida)
        {
            RetornarAlPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        if (colision.CompareTag("Tower"))
        {
            colision.GetComponent<Tower>().TakeDamage(dano);
        }

        // Retornar al pool tras cualquier colisión
        RetornarAlPool();
    }

    private void RetornarAlPool()
    {
        // Devolver al pool si está asignado
        if (pool != null)
        {
            pool.Return(gameObject);
        }
        else
        {
            // Si no hay pool, destruir el proyectil
            Destroy(gameObject);
        }
    }
}