using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;    // El transform del personaje que debe seguir la cámara
    public Vector3 offset;      // La distancia entre la cámara y el personaje
    public float smoothSpeed = 0.125f; // Velocidad de seguimiento suave

    void LateUpdate()
    {
        // Aseguramos que la cámara siga solo los ejes X y Y
        Vector3 desiredPosition = new Vector3(player.position.x, player.position.y, transform.position.z) + offset;

        // Interpola entre la posición actual y la deseada para un movimiento suave
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Actualiza la posición de la cámara
        transform.position = smoothedPosition;

        // Asegura que la cámara siempre mire al personaje (opcional)
        // Si no quieres que la cámara rote, desactiva la línea siguiente
        transform.LookAt(player);
    }
}
