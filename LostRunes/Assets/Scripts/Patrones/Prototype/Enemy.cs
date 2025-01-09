using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class Enemy : MonoBehaviour
{
    public string enemyName;           // Nombre del enemigo
    public float maxHealth = 100f;     // Vida máxima
    private float currentHealth;       // Vida actual
    public GameObject healthBarPrefab; // Prefab de barra de vida
    public float damage;               // Daño del enemigo
    public float speed;                // Velocidad del enemigo

    private Slider healthBar;
    private Transform healthBarTransform;

    public event Action OnDeath;       // Evento para notificar la muerte del enemigo
    private EnemyPool pool;            // Referencia al pool de enemigos

    void Start()
    {
        SetupHealthBar();
    }

    // Inicializar barra de vida
    private void SetupHealthBar()
    {
        // Crear barra de vida
        GameObject healthBarInstance = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, transform);
        healthBar = healthBarInstance.GetComponentInChildren<Slider>();
        healthBarTransform = healthBarInstance.transform;

        // Configurar valores iniciales de la barra de vida
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void Initialize(float health, EnemyPool pool)
    {
        this.pool = pool;             // Asignar referencia al pool
        currentHealth = health;       // Reiniciar la vida del enemigo
        UpdateHealthBar();            // Actualizar la barra de vida
        gameObject.SetActive(true);   // Activar el enemigo
    }

    public void TakeDamage(float damage)
    {
        // Reducir vida
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Actualizar barra de vida
        UpdateHealthBar();

        // Destruir al enemigo si la vida llega a 0
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
    }

    private void Die()
    {
        // Notificar muerte para las oleadas
        OnDeath?.Invoke();

        // Devolver al pool en lugar de destruir
        if (pool != null)
        {
            pool.Return(gameObject);
        }
        else
        {
            Destroy(gameObject); // Backup en caso de no tener pool asignado
        }
    }

    // Método para el movimiento básico
    public virtual void Move(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        FlipSprite(targetPosition); // Girar el sprite
    }

    private void FlipSprite(Vector3 targetPosition)
    {
        // Girar el sprite según la posición del objetivo
        if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(3.22f, 3.22f, 3.22f); // Derecha
        }
        else
        {
            transform.localScale = new Vector3(-3.22f, 3.22f, 3.22f); // Izquierda
        }
    }

    // Método abstracto para clonar (puedes mantenerlo si es necesario)
    public abstract Enemy Clone();
}
