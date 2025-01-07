using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPool : MonoBehaviour, IObjectPool<GameObject>
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
            arrow.transform.SetParent(transform); // Asignar ArrowPool como padre
            arrowPool.Enqueue(arrow);
        }
    }

    // Obtener un objeto del pool
    public GameObject Get()
    {
        GameObject arrow;

        if (arrowPool.Count > 0)
        {
            arrow = arrowPool.Dequeue();
        }
        else
        {
            // Si no hay flechas disponibles, crear una nueva
            arrow = Instantiate(arrowPrefab);
        }

        arrow.SetActive(true);
        arrow.transform.SetParent(transform); // Mantener como hijo del pool

        return arrow;
    }

    // Devolver un objeto al pool
    public void Return(GameObject arrow)
    {
        arrow.SetActive(false);
        arrow.transform.SetParent(transform); // Asegurar que regresa como hijo del pool
        arrowPool.Enqueue(arrow);
    }
}