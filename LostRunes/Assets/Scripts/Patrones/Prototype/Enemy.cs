using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    public string enemyName;      // Nombre del enemigo
    public float maxHealth = 100f;     // Vida m�xima
    private float currentHealth;       // Vida actual
    public GameObject healthBarPrefab;
    public float damage;          // Da�o del enemigo
    public float speed;           // Velocidad del enemigo

    private Slider healthBar;
    private Transform healthBarTransform;

    void Start()
    {
        // Inicializar vida
        currentHealth = maxHealth;

        // Crear barra de vida
        GameObject healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, transform);
        healthBar = healthBarInstance.GetComponentInChildren<Slider>();
        healthBarTransform = healthBarInstance.transform;

        // Inicializar barra de vida
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    // M�todo abstracto para clonar
    public abstract Enemy Clone();

    public void TakeDamage(float damage)
    {
        // Reducir vida
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Actualizar barra de vida
        healthBar.value = currentHealth;

        // Destruir al enemigo si la vida llega a 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // M�todo para destruir al enemigo
    private void Die()
    {
        Destroy(gameObject);
    }

    // M�todo para el movimiento b�sico
    public virtual void Move(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        FlipSprite(targetPosition); // Girar el sprite
    }

    private void FlipSprite(Vector3 targetPosition)
    {
        // Girar el sprite seg�n la posici�n del objetivo
        if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(3.22f, 3.22f, 3.22f); // Derecha
        }
        else
        {
            transform.localScale = new Vector3(-3.22f, 3.22f, 3.22f); // Izquierda
        }
    }
}
