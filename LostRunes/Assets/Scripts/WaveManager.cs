using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public int totalWaves = 10;           // Número total de oleadas
    public float timeBetweenWaves = 5f;  // Tiempo entre oleadas
    public float spawnRadius = 10f;      // Radio para los puntos de aparición

    [SerializeField] private EnemyPool enemyPool; // Pool de enemigos
    [SerializeField] private EnemySpawner enemySpawner; // Referencia al EnemySpawner

    private Transform towerTransform;            // Posición de la torre
    private int currentWave = 0;                 // Contador de la oleada actual
    private int enemiesRemaining = 0;           // Enemigos restantes en la oleada actual

    void Start()
    {
        // Obtener la posición de la torre
        towerTransform = GameObject.FindGameObjectWithTag("Tower").transform;

        if (enemyPool == null)
        {
            Debug.LogError("EnemyPool no asignado al WaveManager.");
        }

        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner no asignado al WaveManager.");
        }

        // Iniciar la secuencia de oleadas
        StartCoroutine(StartWaveSequence());
    }

    private IEnumerator StartWaveSequence()
    {
        while (currentWave < totalWaves)
        {
            // Incrementar la oleada
            currentWave++;
            Debug.Log($"Oleada {currentWave} iniciada.");

            // Calcular el número de enemigos
            int numberOfEnemies = 5 + (1 * currentWave);
            enemiesRemaining = numberOfEnemies;

            // Generar enemigos
            enemySpawner.SpawnEnemies(numberOfEnemies, this);

            // Esperar hasta que todos los enemigos sean derrotados
            while (enemiesRemaining > 0)
            {
                yield return null;
            }

            Debug.Log($"Oleada {currentWave} completada.");

            // Esperar un tiempo antes de la siguiente oleada
            yield return new WaitForSeconds(timeBetweenWaves);
        }

        Debug.Log("¡Todas las oleadas completadas! Victoria.");
        EndGame();
    }

    public void HandleEnemyDeath()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            Debug.Log("Todos los enemigos de la oleada han sido derrotados.");
        }
    }

    private void EndGame()
    {
        Debug.Log("Fin del juego. Implementa tu lógica de victoria aquí.");
        // Aquí puedes mostrar una pantalla de victoria o detener el juego
    }
}