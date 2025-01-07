using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    private Transform target;         // Objetivo del enemigo (jugador o torre)
    public float attackRange = 1.5f;  // Rango de ataque
    public float attackCooldown = 1f; // Tiempo entre ataques

    private float lastAttackTime;     // Control del tiempo del último ataque
    private Animator anim;            // Referencia al Animator

    void Awake()
    {
        anim = GetComponent<Animator>(); // Inicializar el Animator
        target = GameObject.FindGameObjectWithTag("Tower").transform; // Buscar la torre como objetivo
    }

    void Update()
    {
        if (target != null)
        {
            // Obtener la distancia al objetivo
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget > attackRange)
            {
                // Movimiento hacia el objetivo
                Move(target.position); // Usa el método heredado de Enemy
                anim.SetBool("isRunning", true); // Activar animación de correr
            }
            else
            {
                anim.SetBool("isRunning", false); // Detener animación de correr

                // Comprobar si puede atacar (cooldown)
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    Attack(); // Realizar ataque
                    lastAttackTime = Time.time; // Actualizar tiempo del último ataque
                }
            }
        }
    }

    // Método para atacar al objetivo
    private void Attack()
    {
        anim.SetTrigger("Attack"); // Activar la animación de ataque

        // Verificar si el objetivo sigue en rango
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget <= attackRange)
        {
            Tower tower = target.GetComponent<Tower>();
            if (tower != null)
            {
                tower.TakeDamage(damage); // Infligir daño a la torre
            }
        }
    }



    public override Enemy Clone()
    {
        return Instantiate(this); // Clonar el enemigo
    }
}
