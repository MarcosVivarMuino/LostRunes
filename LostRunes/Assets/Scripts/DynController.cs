using UnityEngine;

public class DynController : MonoBehaviour
{
    public float speed = 10f;               // Velocidad del proyectil
    public float damage = 10f;              // Daño del proyectil
    public float lifeTime = 3f;             // Tiempo antes de desaparecer automáticamente

    private Vector3 direction;              // Dirección del proyectil
    private Animator anim;                  // Referencia al Animator
    private bool hasHit = false;            // Control para evitar múltiples impactos

    void Start()
    {
        anim = GetComponent<Animator>();

        // Destruir después de cierto tiempo
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (!hasHit)
        {
            // Mover el proyectil continuamente hacia adelante
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        // Configurar la dirección del proyectil
        direction = newDirection.normalized;

        // Ajustar la rotación hacia el objetivo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si colisiona con la torre
        if (!hasHit && collision.CompareTag("Tower"))
        {
            hasHit = true; // Marcar como impactado para evitar múltiples colisiones

            // Hacer daño a la torre
            Tower tower = collision.GetComponent<Tower>();
            if (tower != null)
            {
                tower.TakeDamage(damage);
            }

            // Destruir después de un pequeño retraso para dar tiempo a la animación
            Destroy(gameObject, 0.2f);
        }
    }
}

