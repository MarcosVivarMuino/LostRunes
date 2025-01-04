using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string enemyName;      // Nombre del enemigo
    public float health;          // Vida del enemigo
    public float damage;          // Da�o del enemigo
    public float speed;           // Velocidad del enemigo

    // M�todo abstracto para clonar
    public abstract Enemy Clone();

    // M�todo para recibir da�o
    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
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
    }
}
