using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string enemyName;      // Nombre del enemigo
    public float health;          // Vida del enemigo
    public float damage;          // Daño del enemigo
    public float speed;           // Velocidad del enemigo

    // Método abstracto para clonar
    public abstract Enemy Clone();

    // Método para recibir daño
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    // Método para destruir al enemigo
    private void Die()
    {
        Destroy(gameObject);
    }

    // Método para el movimiento básico
    public virtual void Move(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
