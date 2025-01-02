using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float velocidad = 10f;  // Velocidad del proyectil
    [SerializeField] private float dano = 10f;       // Da�o que hace el proyectil
    [SerializeField] private float tiempoDeVida = 5f; // Tiempo que el proyectil permanecer� antes de destruirse

    private Vector3 direccion;  // Direcci�n en la que se mover� el proyectil

    // M�todo para configurar la direcci�n del proyectil
    public void ConfigurarDireccion(Vector3 nuevaDireccion)
    {
        direccion = nuevaDireccion;
        RotateArrow();
    }

    private void Start()
    {
        // Destruir el proyectil despu�s de un tiempo si no colisiona
        Destroy(gameObject, tiempoDeVida);
    }

    private void Update()
    {
        // Mover el proyectil en la direcci�n establecida
        transform.position += direccion * velocidad * Time.deltaTime;
    }

    private void RotateArrow()
    {
        // Calcular el �ngulo de rotaci�n en grados
        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // Aplicar la rotaci�n al objeto de la flecha
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        // Detectar colisiones con objetos o enemigos
        if (colision.CompareTag("Enemy"))
        {
            // Aqu� podr�as a�adir el c�digo para hacerle da�o al enemigo
            Destroy(gameObject); // Destruir la flecha al colisionar con un enemigo
        }
    }
}


