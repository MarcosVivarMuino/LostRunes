using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;    // El transform del personaje que debe seguir la c�mara
    public Vector3 offset;      // La distancia entre la c�mara y el personaje
    public float smoothSpeed = 0.125f; // Velocidad de seguimiento suave

    void LateUpdate()
    {
        // Aseguramos que la c�mara siga solo los ejes X y Y
        Vector3 desiredPosition = new Vector3(player.position.x, player.position.y, transform.position.z) + offset;

        // Interpola entre la posici�n actual y la deseada para un movimiento suave
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Actualiza la posici�n de la c�mara
        transform.position = smoothedPosition;

        // Asegura que la c�mara siempre mire al personaje (opcional)
        // Si no quieres que la c�mara rote, desactiva la l�nea siguiente
        transform.LookAt(player);
    }
}
