using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float velocidad = 10f;  // Velocidad del proyectil
    [SerializeField] private float dano = 10f;       // Da�o que hace el proyectil
    [SerializeField] private float tiempoDeVida = 5f; // Tiempo m�ximo antes de volver al pool

    private Vector3 direccion;  // Direcci�n en la que se mover� el proyectil
    private float tiempoTranscurrido = 0f;
    private ArrowPool pool;

    // M�todo para configurar la direcci�n del proyectil
    public void ConfigurarDireccion(Vector3 nuevaDireccion, ArrowPool pool)
    {
        direccion = nuevaDireccion.normalized; // Aseguramos que la direcci�n est� normalizada
        this.pool = pool;
        tiempoTranscurrido = 0f;
    }

    private void Update()
    {
        // Mover el proyectil en la direcci�n establecida
        transform.position += direccion * velocidad * Time.deltaTime;

        // Devolver al pool si excede el tiempo de vida
        tiempoTranscurrido += Time.deltaTime;
        if (tiempoTranscurrido >= tiempoDeVida)
        {
            RetornarAlPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        // Aqu� puedes agregar l�gica adicional, como detectar si colisiona con un enemigo
        //if (colision.CompareTag("Enemy"))  // Ejemplo: detectar enemigos por tag
        //{
        //    colision.GetComponent<Enemy>().TakeDamage(dano); // Aplicar da�o
        //}

        // Retornar al pool tras cualquier colisi�n
        RetornarAlPool();
    }

    private void RetornarAlPool()
    {
        // Si tenemos un pool, devolvemos la flecha all�
        if (pool != null)
        {
            pool.ReturnArrow(gameObject);
        }
        else
        {
            // Si no se ha asignado un pool, destruimos la flecha
            Destroy(gameObject);
        }
    }
}