using UnityEngine;

public class RangedEnemy : Enemy
{
    private Animator anim;                  // Animator para las animaciones
    private Transform target;               // Objetivo (torre)
    private bool isAttacking = false;       // Bandera para controlar el ataque

    public float attackRange = 8f;          // Rango de ataque
    public float attackCooldown = 1.5f;     // Tiempo entre disparos
    private float lastAttackTime = 0f;      // Registro del �ltimo disparo

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
            // Atacar si est� en rango
            Attack();
        }
        else
        {
            // Moverse hacia el objetivo si no est� atacando
            if (!isAttacking)
            {
                Move(target.position); // Usa el m�todo heredado de Enemy
                anim.SetBool("isRunning", true); // Activar animaci�n de correr
            }
        }
    }

    private void Attack()
    {
        // Comprobar si puede atacar seg�n el cooldown
        if (Time.time - lastAttackTime > attackCooldown)
        {
            // Detener el movimiento
            anim.SetBool("isRunning", false);

            // Lanzar animaci�n de ataque
            anim.SetTrigger("Attack");
            isAttacking = true;

            // Disparar el proyectil despu�s de sincronizar con la animaci�n
            Invoke(nameof(FireProjectile), 0.3f);

            // Reiniciar cooldown
            lastAttackTime = Time.time;

            // Reactivar movimiento despu�s del ataque
            Invoke(nameof(ResetAttack), 0.5f);
        }
    }

    private void FireProjectile()
    {
        if (target == null) return;

        // Obtener el proyectil del pool
        GameObject projectile = projectilePool.Get();

        // Desactivar el proyectil primero, antes de activarlo despu�s de la configuraci�n
        projectile.SetActive(false);

        // Configurar la posici�n inicial y la direcci�n del proyectil
        projectile.transform.position = firePoint.position;
        projectile.transform.rotation = Quaternion.identity;

        // Calcular direcci�n hacia el objetivo
        Vector3 direction = (target.position - firePoint.position).normalized;

        // Inicializar el controlador del proyectil
        DynController dyn = projectile.GetComponent<DynController>();
        if (dyn != null)
        {
            dyn.Initialize(direction, damage, projectilePool); // Configurar proyectil
            projectile.SetActive(true); // Activar proyectil despu�s de la configuraci�n
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