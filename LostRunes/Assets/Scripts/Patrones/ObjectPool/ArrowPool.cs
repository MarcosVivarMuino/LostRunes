using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPool : MonoBehaviour
{
    public GameObject arrowPrefab;    // Prefab de la flecha
    public int poolSize = 10;         // Tamaño inicial del pool

    private Queue<GameObject> arrowPool = new Queue<GameObject>();

    void Start()
    {
        // Crear flechas preinstanciadas y desactivadas
        for (int i = 0; i < poolSize; i++)
        {
            GameObject arrow = Instantiate(arrowPrefab);
            arrow.SetActive(false);
            arrowPool.Enqueue(arrow);
        }
    }

    // Obtener una flecha del pool
    public GameObject GetArrow()
    {
        if (arrowPool.Count > 0)
        {
            GameObject arrow = arrowPool.Dequeue();
            arrow.SetActive(true);
            return arrow;
        }
        else
        {
            // Si no hay flechas disponibles, crear una nueva (opcional)
            GameObject newArrow = Instantiate(arrowPrefab);
            return newArrow;
        }
    }

    // Devolver la flecha al pool
    public void ReturnArrow(GameObject arrow)
    {
        arrow.SetActive(false);
        arrowPool.Enqueue(arrow);
    }
}
