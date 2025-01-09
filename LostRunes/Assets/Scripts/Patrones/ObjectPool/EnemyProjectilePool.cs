using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectilePool : MonoBehaviour, IObjectPool<GameObject>
{
    public GameObject projectilePrefab; // Prefab del proyectil enemigo
    public int poolSize = 20;           // Tamaño inicial del pool

    private Queue<GameObject> projectilePool = new Queue<GameObject>();

    void Start()
    {
        // Crear proyectiles preinstanciados y desactivados
        for (int i = 0; i < poolSize; i++)
        {
            GameObject projectile = Instantiate(projectilePrefab);
            projectile.SetActive(false);
            projectile.transform.SetParent(transform); // Asignar como hijo del pool
            projectilePool.Enqueue(projectile);
        }
    }

    // Obtener un proyectil del pool
    public GameObject Get()
    {
        GameObject projectile;

        if (projectilePool.Count > 0)
        {
            projectile = projectilePool.Dequeue();
        }
        else
        {
            // Si no hay proyectiles disponibles, crear uno nuevo
            projectile = Instantiate(projectilePrefab);
        }

        projectile.SetActive(true); // Activar el proyectil después de colocarlo
        projectile.transform.SetParent(transform); // Liberar del pool al activarlo
        return projectile;
    }

    // Devolver un proyectil al pool
    public void Return(GameObject projectile)
    {
        projectile.SetActive(false); // Desactivar el proyectil antes de devolverlo al pool
        projectile.transform.SetParent(transform); // Asegurar que regresa como hijo del pool
        projectilePool.Enqueue(projectile); // Volverlo al pool
    }
}