using UnityEngine;

public class DynController : MonoBehaviour
{
    public float speed = 10f;               // Velocidad del proyectil
    public float damage = 10f;              // Da�o del proyectil
    public float lifeTime = 3f;             // Tiempo antes de desaparecer autom�ticamente

    private Vector3 direction;              // Direcci�n del proyectil
    private Animator anim;                  // Referencia al Animator
    private bool hasHit = false;            // Control para evitar m�ltiples impactos

    void Start()
    {
        anim = GetComponent<Animator>();

        // Destruir despu�s de cierto tiempo
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
        // Configurar la direcci�n del proyectil
        direction = newDirection.normalized;

        // Ajustar la rotaci�n hacia el objetivo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificar si colisiona con la torre
        if (!hasHit && collision.CompareTag("Tower"))
        {
            hasHit = true; // Marcar como impactado para evitar m�ltiples colisiones

            // Hacer da�o a la torre
            Tower tower = collision.GetComponent<Tower>();
            if (tower != null)
            {
                tower.TakeDamage(damage);
            }

            // Destruir despu�s de un peque�o retraso para dar tiempo a la animaci�n
            Destroy(gameObject, 0.2f);
        }
    }
}

