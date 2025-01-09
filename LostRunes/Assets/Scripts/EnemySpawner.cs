using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyPool enemyPool; // Pool de enemigos
    [SerializeField] private Transform tower;    // Referencia a la torre
    [SerializeField] private float spawnRadius = 10f; // Radio para los puntos de aparición

    public void SpawnEnemies(int numberOfEnemies, WaveManager waveManager)
    {
        Debug.Log($"Spawneando {numberOfEnemies} enemigos en la oleada.");

        for (int i = 0; i < numberOfEnemies; i++)
        {
            SpawnEnemy(waveManager);
        }
    }

    private void SpawnEnemy(WaveManager waveManager)
    {
        // Obtener un enemigo del pool
        GameObject enemy = enemyPool.Get();

        if (enemy != null)
        {
            // Calcular una posición aleatoria alrededor de la torre
            Vector3 spawnPosition = GetRandomSpawnPoint(tower.position, spawnRadius);

            // Configurar la posición inicial y activar el enemigo
            enemy.transform.position = spawnPosition;
            enemy.SetActive(true);

            // Inicializar al enemigo
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.Initialize(enemyScript.maxHealth, enemyPool); // Configuración del enemigo

                // Suscribirse al evento de muerte para notificar al WaveManager
                enemyScript.OnDeath += waveManager.HandleEnemyDeath;
            }
        }
        else
        {
            Debug.LogWarning("No hay suficientes enemigos disponibles en el pool.");
        }
    }

    private Vector3 GetRandomSpawnPoint(Vector3 center, float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float distance = Random.Range(0f, radius);
        float x = center.x + Mathf.Cos(angle) * distance;
        float y = center.y + Mathf.Sin(angle) * distance;
        return new Vector3(x, y, center.z);
    }
}