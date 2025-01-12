using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f; // Vida m�xima de la torre
    [SerializeField] private Slider healthBar;      // Barra de vida (UI)

    private float currentHealth; // Vida actual de la torre

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar(); // Actualizar la barra al inicio
    }

    // M�todo para recibir da�o
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

    // M�todo para actualizar la barra de vida
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / maxHealth;
        }
    }

    // M�todo para manejar la destrucci�n de la torre
    private void DestroyTower()
    {

        UIManager.Instance.GoToDefeat();
        Destroy(gameObject); // Destruir el objeto de la torre
    }
}

