using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynController : MonoBehaviour
{
    public float speed = 10f;           // Velocidad del proyectil
    public float damage = 10f;          // Da�o del proyectil
    public float lifeTime = 3f;         // Tiempo antes de desaparecer autom�ticamente

    private Vector3 direction;          // Direcci�n del proyectil
    private Animator anim;              // Referencia al Animator
    private bool hasHit = false;        // Control para evitar m�ltiples impactos

    void Start()
    {
        anim = GetComponent<Animator>();

        // Destruir despu�s de cierto tiempo
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        if (!hasHit)
        {
            // Mover el proyectil continuamente
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        // Configurar la direcci�n del proyectil
        direction = newDirection.normalized;

        // Ajustar la rotaci�n hacia el objetivo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasHit)
        {
            hasHit = true;

            // Si impacta, desactiva el movimiento
            speed = 0;

            // Destruir el proyectil despu�s del impacto (o devolverlo al pool si usas Object Pooling)
            Destroy(gameObject, 0.2f);
        }
    }
}
