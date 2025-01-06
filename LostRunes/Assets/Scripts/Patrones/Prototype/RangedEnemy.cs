using UnityEngine;

public class RangedEnemy : Enemy
{
    private Animator anim;                  // Animator para las animaciones
    private Transform target;               // Objetivo (jugador)
    private bool isAttacking = false;       // Bandera para controlar el ataque

    public float attackRange = 8f;          // Rango de ataque
    public float attackCooldown = 1.5f;     // Tiempo entre disparos
    private float lastAttackTime = 0f;      // Registro del �ltimo disparo

    public GameObject projectilePrefab;     // Prefab del proyectil
    public Transform firePoint;             // Punto desde donde dispara el proyectil
    public float projectileSpeed = 10f;     // Velocidad del proyectil

    private void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Tower").transform; // Buscar jugador por tag
    }

    private void Update()
    {
        // Si el enemigo est� muerto, no hacer nada
        if (health <= 0) return;

        // Calcular distancia al jugador
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            // Si est� en rango, atacar
            Attack();
        }
        else
        {
            // Si no est� atacando, moverse hacia el jugador
            if (!isAttacking)
            {
                Move(target.position);
                anim.SetBool("isRunning", true); // Activar animaci�n de correr
            }
        }
    }

    public override void Move(Vector3 targetPosition)
    {
        // Mover hacia el objetivo
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        FlipSprite(targetPosition); // Girar el sprite
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime > attackCooldown)
        {
            // Detener el movimiento
            anim.SetBool("isRunning", false);

            // Lanzar animaci�n de ataque
            anim.SetTrigger("Attack");
            isAttacking = true;

            // Disparar el proyectil
            Invoke(nameof(FireProjectile), 0.3f); // Retraso para sincronizar con la animaci�n

            // Reiniciar cooldown
            lastAttackTime = Time.time;

            // Reactivar movimiento despu�s del ataque
            Invoke(nameof(ResetAttack), 0.5f);
        }
    }

    private void FireProjectile()
    {
        if (target == null) return;

        // Crear el proyectil
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Calcular direcci�n hacia el jugador
        Vector3 direction = (target.position - firePoint.position).normalized;

        // Configurar el proyectil
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.velocity = direction * projectileSpeed;

        // Girar el proyectil seg�n la direcci�n
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    private void FlipSprite(Vector3 targetPosition)
    {
        // Girar el sprite seg�n la posici�n del objetivo
        if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(2.18f, 2.18f, 2.18f); // Mirando a la derecha
        }
        else
        {
            transform.localScale = new Vector3(-2.18f, 2.18f, 2.18f); // Mirando a la izquierda
        }
    }

    public override Enemy Clone()
    {
        return Instantiate(this); // Clonar el enemigo
    }
}