using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ArrowController : MonoBehaviour
{
    [SerializeField] private float velocidad = 10f;  // Velocidad del proyectil
    [SerializeField] private float dano = 10f;       // Daño que hace el proyectil
    [SerializeField] private float tiempoDeVida = 5f; // Tiempo que el proyectil permanecerá antes de destruirse

    private Vector3 direccion;  // Dirección en la que se moverá el proyectil

    // Método para configurar la dirección del proyectil
    public void ConfigurarDireccion(Vector3 nuevaDireccion)
    {
        direccion = nuevaDireccion;
        RotateArrow();
    }

    private void Start()
    {
        // Destruir el proyectil después de un tiempo si no colisiona
        Destroy(gameObject, tiempoDeVida);
    }

    private void Update()
    {
        // Mover el proyectil en la dirección establecida
        transform.position += direccion * velocidad * Time.deltaTime;
    }

    private void RotateArrow()
    {
        // Calcular el ángulo de rotación en grados
        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // Aplicar la rotación al objeto de la flecha
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void OnTriggerEnter2D(Collider2D colision)
    {
        // Detectar colisiones con objetos o enemigos
        if (colision.CompareTag("Enemy"))
        {
            // Aquí podrías añadir el código para hacerle daño al enemigo
            Destroy(gameObject); // Destruir la flecha al colisionar con un enemigo
        }
    }
}


