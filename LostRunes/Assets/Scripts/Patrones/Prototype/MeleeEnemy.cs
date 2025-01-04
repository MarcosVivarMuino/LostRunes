using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    private Animator anim;             // Referencia al Animator
    private Transform target;          // Objetivo del enemigo
    private bool isAttacking = false;  // Bandera para controlar el ataque

    public float attackRange = 1.5f;   // Rango de ataque
    public float attackCooldown = 1f;  // Tiempo entre ataques
    private float lastAttackTime = 0f; // Registro del último ataque

    private void Start()
    {
        anim = GetComponent<Animator>(); // Obtener el Animator
        target = GameObject.FindGameObjectWithTag("Tower").transform; // Buscar al jugador por tag
    }

    private void Update()
    {
        // Si el enemigo está muerto, no hacer nada
        if (health <= 0) return;

        // Calcular distancia al objetivo
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            // Si está en rango de ataque
            Attack();
        }
        else
        {
            // Mover hacia el objetivo si no está atacando
            if (!isAttacking)
            {
                Move(target.position);
                anim.SetBool("isRunning", true); // Animación de correr
            }
        }
    }

    public override void Move(Vector3 targetPosition)
    {
        // Mover hacia el jugador
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        FlipSprite(targetPosition); // Girar el sprite si es necesario
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            // Detener el movimiento
            anim.SetBool("isRunning", false);

            // Lanzar animación de ataque
            anim.SetTrigger("Attack");
            isAttacking = true;

            // Reiniciar cooldown
            lastAttackTime = Time.time;

            // Simular daño al jugador
            target.GetComponent<Tower>().TakeDamage(damage);

            // Reactivar movimiento después del ataque
            Invoke(nameof(ResetAttack), 0.5f); // Esperar medio segundo
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
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

    public override Enemy Clone()
    {
        return Instantiate(this); // Clonar el enemigo
    }
}
