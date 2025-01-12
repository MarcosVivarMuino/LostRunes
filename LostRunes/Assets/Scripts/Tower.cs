using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Vida máxima de la torre
    [SerializeField] private Slider healthBar;      // Barra de vida (UI)

    private float currentHealth; // Vida actual de la torre

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar(); // Actualizar la barra al inicio
    }

    // Método para recibir daño
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Limitar el rango entre 0 y maxHealth
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            DestroyTower();
        }
    }

    // Método para actualizar la barra de vida
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    // Método para manejar la destrucción de la torre
    private void DestroyTower()
    {

        UIManager.Instance.GoToDefeat();
        Destroy(gameObject); // Destruir el objeto de la torre
    }
}

