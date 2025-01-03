using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCommand : ICommand
{
    private Transform bowPivot;             // Pivote del arco
    private ArrowPool arrowPool;            // Pool de flechas
    private Vector3 direction;              // Direcci�n del disparo
    private Animator bowAnimator;           // Animator del arco

    public FireCommand(Transform bowPivot, ArrowPool arrowPool, Vector3 direction, Animator bowAnimator)
    {
        this.bowPivot = bowPivot;
        this.arrowPool = arrowPool;
        this.direction = direction;
        this.bowAnimator = bowAnimator;
    }

    public void Execute()
    {
        // Reproducir la animaci�n del arco
        if (bowAnimator != null)
        {
            bowAnimator.SetTrigger("Shoot");
        }

        // Obtener una flecha del pool
        GameObject arrow = arrowPool.GetArrow();

        // Configurar posici�n y rotaci�n de la flecha
        arrow.transform.position = bowPivot.position;

        // Calcular la rotaci�n necesaria para apuntar hacia el rat�n
        arrow.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

        // Configurar la direcci�n de la flecha en ArrowController
        ArrowController arrowController = arrow.GetComponent<ArrowController>();
        if (arrowController != null)
        {
            arrowController.ConfigurarDireccion(direction, arrowPool);
        }
    }
}