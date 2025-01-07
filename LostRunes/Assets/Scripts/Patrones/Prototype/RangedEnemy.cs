using UnityEngine;

public class RangedEnemy : Enemy
{
    private Animator anim;                  // Animator para las animaciones
    private Transform target;               // Objetivo (torre)
    private bool isAttacking = false;       // Bandera para controlar el ataque

    public float attackRange = 8f;          // Rango de ataque
    public float attackCooldown = 1.5f;     // Tiempo entre disparos
    private float lastAttackTime = 0f;      // Registro del último disparo

    public Transform firePoint;             // Punto desde donde dispara el proyectil
    public float projectileSpeed = 10f;     // Velocidad del proyectil

    private EnemyProjectilePool projectilePool; // Referencia al pool de proyectiles enemigos

    void Awake()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Tower").transform; // Buscar torre como objetivo

        // Buscar el pool en la escena
        projectilePool = FindObjectOfType<EnemyProjectilePool>();

        if (projectilePool == null)
        {
            Debug.LogError("EnemyProjectilePool no encontrado en la escena.");
        }
    }

    void Update()
    {
        // Calcular distancia al objetivo
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= attackRange)
        {
            // Atacar si está en rango
            Attack();
        }
        else
        {
            // Moverse hacia el objetivo si no está atacando
            if (!isAttacking)
            {
                Move(target.position); // Usa el método heredado de Enemy
                anim.SetBool("isRunning", true); // Activar animación de correr
            }
        }
    }

    private void Attack()
    {
        // Comprobar si puede atacar según el cooldown
        if (Time.time - lastAttackTime > attackCooldown)
        {
            // Detener el movimiento
            anim.SetBool("isRunning", false);

            // Lanzar animación de ataque
            anim.SetTrigger("Attack");
            isAttacking = true;

            // Disparar el proyectil después de sincronizar con la animación
            Invoke(nameof(FireProjectile), 0.3f);

            // Reiniciar cooldown
            lastAttackTime = Time.time;

            // Reactivar movimiento después del ataque
            Invoke(nameof(ResetAttack), 0.5f);
        }
    }

    private void FireProjectile()
    {
        if (target == null) return;

        // Obtener el proyectil del pool
        GameObject projectile = projectilePool.Get();

        // Desactivar el proyectil primero, antes de activarlo después de la configuración
        projectile.SetActive(false);

        // Configurar la posición inicial y la dirección del proyectil
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = Quaternion.identity;

        // Calcular dirección hacia el objetivo
        Vector3 direction = (target.position - firePoint.position).normalized;

        // Inicializar el controlador del proyectil
        DynController dyn = projectile.GetComponent<DynController>();
        if (dyn != null)
        {
            dyn.Initialize(direction, damage, projectilePool); // Configurar proyectil
            projectile.SetActive(true); // Activar proyectil después de la configuración
        }
        else
        {
            Debug.LogError("El proyectil no tiene un DynController.");
        }
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    public override Enemy Clone()
    {
        return Instantiate(this); // Clonar el enemigo
    }
}