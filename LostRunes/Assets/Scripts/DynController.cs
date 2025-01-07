using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DynController : MonoBehaviour
{
    [SerializeField] private float velocidad = 10f;       // Velocidad del proyectil
    [SerializeField] private float dano = 10f;            // Da�o que hace el proyectil
    [SerializeField] private float tiempoDeVida = 5f;     // Tiempo m�ximo antes de volver al pool

    private Vector3 direccion;                            // Direcci�n del proyectil
    private float tiempoTranscurrido = 0f;                // Tiempo en vuelo
    private EnemyProjectilePool pool;                     // Referencia al pool de proyectiles enemigos

    // Inicializar el proyectil con direcci�n, objetivo y pool
    public void Initialize(Vector3 nuevaDireccion, float dano, EnemyProjectilePool pool)
    {
        direccion = nuevaDireccion.normalized; // Normalizar direcci�n
        this.dano = dano;                      // Asignar da�o
        this.pool = pool;                      // Asignar pool
        tiempoTranscurrido = 0f;               // Reiniciar el tiempo de vida

        // Desactivar el proyectil para asegurarse de que no se active antes de ser completamente inicializado
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // Solo cuando el proyectil se habilita, empezar a moverse
        tiempoTranscurrido = 0f; // Reiniciar el tiempo transcurrido cada vez que el proyectil es activado
    }

    private void Update()
    {
        // Mover el proyectil en la direcci�n establecida
        transform.position += direccion * velocidad * Time.deltaTime;

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

        // Retornar al pool tras cualquier colisi�n
        RetornarAlPool();
    }

    private void RetornarAlPool()
    {
        // Devolver al pool si est� asignado
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